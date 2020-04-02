using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FuzzySystemGUI
{
    class AirLandingSystem
    {
        public double windPower; // сила и направление ветра
        public double weight; // дополнительный вес самолета
        public double runwayIncline; // уклон ВПП (+ нисходящий, - восходящий)
        
        public string flapOutput;
        public string angelOutput;
        public string brakeOutput;

        List<string> fuzzyParams; // нечеткое представление параметров
        Dictionary<string, string> rules;

        // ВХОДНЫЕ ПАРАМЕТРЫ

        /// <summary>
        /// СИЛА ВЕТРА
        ///     BN - сильный отрицательный      [ -11 -15 ]
        ///     MN - средний отрицательный      [ -6 -10 ]
        ///     SN - слабый отрицательный       [ 0 -5 ]
        ///     N  - нет ветра                  0
        ///     SP - слабый положительный       [ 0 5 ]
        ///     MP - средний положительный      [ 6 10 ]
        ///     BP - сильный положительный      [ 11 15 ]
        /// 
        /// </summary>

        List<MembershipFunction> windPowerFunctions; // функции принадлежности для первого параметра

        /// <summary>
        /// ДОПОЛНИТЕЛЬНЫЙ ВЕС САМОЛЕТА
        ///     N  - нет                    0
        ///     VS - очень маленький        [ 0 500 ]
        ///     S  - маленький              [ 500 1000 ]
        ///     M  - средний                [ 1000 1800 ]
        ///     B  - большой                [ 1800 2500 ]
        ///     VB - очень большой          [ 2500 3000 ]
        /// 
        /// </summary>

        List<MembershipFunction> weigthFunctions; // функции принадлежности для второго параметра

        /// <summary>
        /// УКЛОН ВПП
        ///     BN - большой отрицательный      [ -3 -6 ]
        ///     SN - небольшой отрицательный    [ 0 -3 ]
        ///     N  - ровная полоса              0
        ///     SP - небольшой положительный    [ 0 3 ]
        ///     BP - большой положительный      [ 3 6 ]
        /// 
        /// </summary>

        List<MembershipFunction> runwayInclineFunctions; // функции принадлежности для третьего параметра


        // ВЫХОДНЫЕ ПАРАМЕТРЫ

        /// <summary>
        /// ПОЛОЖЕНИЕ ЗАКРЫЛКОВ
        ///     N   - открыты                   [  нет увеличения длины пути   ]
        ///     OQC - закрыты на четверть       [ увеличение длины пути на 5% ]
        ///     HC  - закрыты на половину       [ увеличение длины пути на 10% ]
        ///     TQC - закрыты на три четверти   [ увеличение длины пути на 15% ]
        ///     C   - закрыты                   [ увеличение длины пути на 20% ]
        /// 
        /// </summary>

        Dictionary<string, string> flapValues;

        /// <summary>
        /// УГОЛ ПОСАДКИ
        ///     BN - большой отрицательный      [ 2-5   градусов    - увеличение длины пути на 25% ]
        ///     MN - средний отрицательный      [ 5-7   градусов    - увеличение длины пути на 15% ]    
        ///     SN - небольшой отрицательный    [ 7-9   градусов    - увеличение длины пути на 5%  ]
        ///     N  - нормальный                 [ 9     градусов    - нет увеличения длины пути    ]
        ///     SP - небольшой положительный    [ 9-11  градусов    - уменьшение длины пути на 5%  ]
        ///     MP - средний положительный      [ 11-13 градусов    - уменьшение длины пути на 15% ]
        ///     BP - большой отрицательный      [ 13-16 градусов    - уменьшение длины пути на 25% ]
        /// 
        /// </summary>

        Dictionary<string, string> landAngelValues;

        /// <summary>
        /// СИЛА ТОРМОЖЕНИЯ
        ///     N  - нормальная             [ 10      - длина пути не изменяется ]
        ///     VS - очень маленькая        [ 10-20   - длина пути уменьшается на 5% ]
        ///     S  - маленькая              [ 20-35   - длина пути уменьшается на 10% ]
        ///     M  - средняя                [ 35-60   - длина пути уменьшается на 20% ]
        ///     B  - высокая                [ 60-85   - длина пути уменьшается на 30% ]
        ///     VB - очень высокая          [ 85-100  - длина пути уменьшается на 40% ]
        /// 
        /// </summary>

        Dictionary<string, string> brakeValues;


        public AirLandingSystem(double windPower, double weight, double runwayIncline)
        {
            this.windPower = windPower;
            this.weight = weight;
            this.runwayIncline = runwayIncline;

            init();
            loadRules();
        }

        public void execute()
        {
            fuzzification();

            string decision = getDecision();

            string[] outputParams = decision.Split(' ');

            flapOutput = flapValues[outputParams[0]];
            angelOutput = landAngelValues[outputParams[1]];
            brakeOutput = brakeValues[outputParams[2]];
        }


        private void init()
        {
            windPowerFunctions = new List<MembershipFunction>();
            windPowerFunctions.Add(new TriangularFunction(new double[] { -15, -13, -10 }, "BN"));
            windPowerFunctions.Add(new TrapezoidalFunction(new double[] { -12, -9, -7, -5 }, "MN"));
            windPowerFunctions.Add(new TrapezoidalFunction(new double[] { -7, -4, -2, 0 }, "SN"));
            windPowerFunctions.Add(new MembershipPoint(new double[] { 0 }, "N"));
            windPowerFunctions.Add(new TrapezoidalFunction(new double[] { 0, 2, 4, 7 }, "SP"));
            windPowerFunctions.Add(new TrapezoidalFunction(new double[] { 5, 7, 9, 12 }, "MP"));
            windPowerFunctions.Add(new TriangularFunction(new double[] { 10, 13, 15 }, "BP"));


            weigthFunctions = new List<MembershipFunction>();
            weigthFunctions.Add(new MembershipPoint(new double[] { 0 }, "N"));
            weigthFunctions.Add(new TriangularFunction(new double[] { 0, 300, 600 }, "VS"));
            weigthFunctions.Add(new TrapezoidalFunction(new double[] { 400, 700, 1000, 1200 }, "S"));
            weigthFunctions.Add(new TrapezoidalFunction(new double[] { 900, 1400, 1700, 2000 }, "M"));
            weigthFunctions.Add(new TrapezoidalFunction(new double[] { 1700, 2100, 2400, 2600 }, "B"));
            weigthFunctions.Add(new TriangularFunction(new double[] { 2400, 2800, 3000 }, "VB"));

            runwayInclineFunctions = new List<MembershipFunction>();
            runwayInclineFunctions.Add(new TriangularFunction(new double[] { -6, -4.5, -2 }, "BN"));
            runwayInclineFunctions.Add(new TrapezoidalFunction(new double[] { -4, -2.5, -1, 0 }, "SN"));
            runwayInclineFunctions.Add(new MembershipPoint(new double[] { 0 }, "N"));
            runwayInclineFunctions.Add(new TrapezoidalFunction(new double[] { 0, 1, 2.5, 4 }, "SP"));
            runwayInclineFunctions.Add(new TriangularFunction(new double[] { 2, 4.5, 6 }, "BP"));


            flapValues = new Dictionary<string, string>();
            flapValues.Add("N", "открыты");
            flapValues.Add("OQC", "закрыты на четверть");
            flapValues.Add("HC", "закрыты на половину");
            flapValues.Add("TQC", "закрыты на три четверти");
            flapValues.Add("C", "закрыты");


            landAngelValues = new Dictionary<string, string>();
            landAngelValues.Add("BN", "3 градуса");
            landAngelValues.Add("MN", "6 градусов");
            landAngelValues.Add("SN", "8 градусов");
            landAngelValues.Add("N", "9 градусов");
            landAngelValues.Add("SP", "10 градусов");
            landAngelValues.Add("MP", "12 градусов");
            landAngelValues.Add("BP", "15 градусов");


            brakeValues = new Dictionary<string, string>();
            brakeValues.Add("N", "10%");
            brakeValues.Add("VS", "20%");
            brakeValues.Add("S", "30%");
            brakeValues.Add("M", "50%");
            brakeValues.Add("B", "70%");
            brakeValues.Add("VB", "85%");
        }

        private void loadRules()
        {
            FileStream fs = new FileStream("rules.txt", FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(fs);

            rules = new Dictionary<string, string>();

            while (!reader.EndOfStream)
            {
                string[] rule = reader.ReadLine().Split(' ');

                rules.Add(rule[0] + " " + rule[1] + " " + rule[2], rule[3] + " " + rule[4] + " " + rule[5]);
            }

            reader.Close();
            fs.Close();
        }

        private void fuzzification()
        {
            fuzzyParams = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                fuzzyParams.Add("");
            }

            double maxProbability = -1.0;
            double currentProbability;

            for (int i = 0; i < windPowerFunctions.Count; i++)
            {
                currentProbability = windPowerFunctions[i].calculateProbability(windPower);
                if (currentProbability >= maxProbability)
                {
                    fuzzyParams[0] = windPowerFunctions[i].Term;
                    maxProbability = currentProbability;
                }
            }

            maxProbability = -1.0;

            for (int i = 0; i < weigthFunctions.Count; i++)
            {
                currentProbability = weigthFunctions[i].calculateProbability(weight);
                if (currentProbability >= maxProbability)
                {
                    fuzzyParams[1] = weigthFunctions[i].Term;
                    maxProbability = currentProbability;
                }
            }

            maxProbability = -1.0;

            for (int i = 0; i < runwayInclineFunctions.Count; i++)
            {
                currentProbability = runwayInclineFunctions[i].calculateProbability(runwayIncline);
                if (currentProbability >= maxProbability)
                {
                    fuzzyParams[2] = runwayInclineFunctions[i].Term;
                    maxProbability = currentProbability;
                }
            }
        }

        private string getDecision()
        {
            string key = "";

            for (int i = 0; i < fuzzyParams.Count; i++)
            {
                if (i != fuzzyParams.Count - 1)
                    key += fuzzyParams[i] + " ";
                else
                    key += fuzzyParams[i];
            }

            return rules[key];
        }
    }
}

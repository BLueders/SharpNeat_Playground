﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using log4net.Config;
using SharpNeat.EvolutionAlgorithms;
using SharpNeat.Genomes.Neat;
using SharpNeat.Core;
using SharpNeat.Phenomes;

namespace NeatSine
{
    class Program
    {
        static NeatEvolutionAlgorithm<NeatGenome> _ea;
        const string CHAMPION_FILE = "sine_champion.xml";

        private static void Main(string[] args)
        {
            // Initialise log4net (log to console).
            XmlConfigurator.Configure(new FileInfo("log4net.properties"));

            // Experiment classes encapsulate much of the nuts and bolts of setting up a NEAT search.
            SineExperiment experiment = new SineExperiment(true, new SineEvaluator(50));

            // Load config XML.
            XmlDocument xmlConfig = new XmlDocument();
            xmlConfig.Load("sine.config.xml");
            experiment.Initialize("DA MAGIC SUPER SINE", xmlConfig.DocumentElement);

            // Create evolution algorithm and attach update event.
            _ea = experiment.CreateEvolutionAlgorithm();
            _ea.UpdateEvent += ea_UpdateEvent;

            // Start algorithm (it will run on a background thread).
            _ea.StartContinue();

            // Hit return to quit.
            Console.ReadLine();

            // Get a genome decoder that can convert genomes to phenomes.
            IGenomeDecoder<NeatGenome, IBlackBox> decoder = experiment.CreateGenomeDecoder();

            // Get the champion genome
            NeatGenome champion = _ea.CurrentChampGenome;

            TestGenome(decoder, champion);
            
        }

        private static void TestGenome(IGenomeDecoder<NeatGenome, IBlackBox> decoder, NeatGenome genome)
        {
            // Decode the genome into a phenome (neural network).
            IBlackBox phenome = decoder.Decode(genome);

            List<string> lines = new List<string>();

            double v = 0f;
            while (v < Math.PI * 2f)
            {
                phenome.ResetState();

                phenome.InputSignalArray[0] = v;

                phenome.Activate();

                double result = phenome.OutputSignalArray[0];

                lines.Add(string.Format("{0}\t{1}", v, result));

                v += 0.1d;
            }

            System.IO.File.WriteAllLines(@"output.csv", lines);
        }

        private static void ea_UpdateEvent(object sender, EventArgs e)
        {
            Console.WriteLine("gen={0:N0} bestFitness={1:N6}", _ea.CurrentGeneration, _ea.Statistics._maxFitness);

            // Save the best genome to file
            XmlDocument doc = NeatGenomeXmlIO.SaveComplete(new List<NeatGenome>() { _ea.CurrentChampGenome }, false);
            doc.Save(CHAMPION_FILE);
        }
    }
}

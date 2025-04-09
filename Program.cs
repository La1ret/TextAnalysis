using NUnitLite;

namespace TextAnalysis;

public static class Program
{
    public static void Main(string[] args)
    {
        // var testsToRun = new[] //Тесты позволяющие проверять работу отдельных модулей программы
        //{
        //     //"TextAnalysis.SentencesParser_Tests",
        //     //"TextAnalysis.FrequencyAnalysis_Tests",
        //     "TextAnalysis.TextGenerator_Tests",
        // };
        // new AutoRun().Execute(new[]
        // {
        //     //"--stoponerror", // Останавливать после первого же непрошедшего теста. Закомментируйте, чтобы увидеть все падающие тесты
        //     "--noresult",
        //     "--test=" + string.Join(",", testsToRun)
        // });


        var text = File.ReadAllText("FullTextMoomins.txt");
        var sentences = SentencesParserTask.ParseSentences(text);
        var frequency = FrequencyAnalysisTask.GetMostFrequentNextWords(sentences);
        var frequency2 = FrequencyAnalysisTask.GetNextWords(sentences);

        while (true)
        {
            Console.Write("\nВведите первое слово: ");
            var beginning = Console.ReadLine();
            if (string.IsNullOrEmpty(beginning)) return;
            Console.WriteLine("\nНаиболее вероятное продолжение:");
            var phrase = TextGeneratorTask.ContinuePhrase(frequency, beginning.ToLower(), 50);
            Console.WriteLine(phrase);

            Console.WriteLine("\nПродолжение такста основанное на распределении вероятности:");
            phrase = TextGeneratorTask.ContinuePhraseRandomNextWords(frequency2, beginning.ToLower(), 50);
            Console.WriteLine(phrase);
        }
    }
}

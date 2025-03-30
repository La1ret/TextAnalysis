using NUnitLite;

namespace TextAnalysis;

public static class Program
{
    public static void Main(string[] args)
    {
        var text = File.ReadAllText("HarryPotterText.txt");
        var sentences = SentencesParserTask.ParseSentences(text);
        var frequency = FrequencyAnalysisTask.GetMostFrequentNextWords(sentences);

        while (true)
        {
            Console.Write("Введите первое слово: ");
            var beginning = Console.ReadLine();
            if (string.IsNullOrEmpty(beginning)) return;
            var phrase = TextGeneratorTask.ContinuePhrase(frequency, beginning.ToLower(), 50);
            Console.WriteLine(phrase);
        }
    }
}
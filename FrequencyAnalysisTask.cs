using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Collections.Generic;
using System.Text;

namespace TextAnalysis;

static class FrequencyAnalysisTask
{
    public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
    {
        var bigrams = GetNgramms(text, 2);
        var trigrams = GetNgramms(text, 3);
        var fourgrams = GetNgramms(text, 4);
        var result = bigrams.Concat(trigrams).ToDictionary(x => x.Key, x => x.Value).Concat(fourgrams).ToDictionary(x => x.Key, x => x.Value);
        return result;
    }

    static Dictionary<string, string> GetNgramms(List<List<string>> text, int n)
    {
        var ngrams = new Dictionary<string, string>();
        var frequency = new Dictionary<string, int>();

        foreach (var words in text)
        {
            for (int i = 0; i < words.Count - n + 1; i++)
            {
                string ngramKey = GetNgramKey(n, words, i), ngram = $"{ngramKey} {words[i + n - 1]}";
                frequency.TryAdd(ngram, 0);
                frequency[ngram] += 1;

                ngrams.TryAdd(ngramKey, words[i + n - 1]);
                frequency.TryAdd($"{ngramKey} {ngrams[ngramKey]}", 1);

                if (frequency[ngram] > frequency[$"{ngramKey} {ngrams[ngramKey]}"] ||
                    (frequency[ngram] == frequency[$"{ngramKey} {ngrams[ngramKey]}"] && string.Compare(words[i + n - 1], ngrams[ngramKey]) == -1))
                    ngrams[ngramKey] = words[i + n - 1];
            }
        }
        return ngrams;
    }

    static string GetNgramKey(int n, List<string> words, int i)
    {
        var ngramKey = new StringBuilder( $"{words[i]}" );
        for (int j =  1; j < n - 1; j++)
        {
            ngramKey.Append($" {words[i+j]}");
        }
        return ngramKey.ToString();
    }
}
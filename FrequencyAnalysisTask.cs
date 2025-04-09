using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Collections.Generic;
using System.Text;

namespace TextAnalysis;

static class FrequencyAnalysisTask
{
    public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
    {
        var bigrams = GetNgramsOnlyContinuation(text, 2);
        var trigrams = GetNgramsOnlyContinuation(text, 3);
        var fourgrams = GetNgramsOnlyContinuation(text, 4);
        var fivegrams = GetNgramsOnlyContinuation(text, 5);
        var result = bigrams.Concat(trigrams).ToDictionary(x => x.Key, x => x.Value).Concat(fourgrams).ToDictionary(x => x.Key, x => x.Value)
            .Concat(fivegrams).ToDictionary(x => x.Key, x => x.Value);
        return result;
    }

    public static Dictionary<string,List<string>> GetNextWords(List<List<string>> text)
    {
        var bigrams = GetNgramsManyContinuation(text, 2);
        var trigrams = GetNgramsManyContinuation(text, 3);
        var fourgrams = GetNgramsManyContinuation(text, 4);
        var fivegrams = GetNgramsManyContinuation(text, 5);
        var result = bigrams.Concat(trigrams).ToDictionary(x => x.Key, x => x.Value).Concat(fourgrams).ToDictionary(x => x.Key, x => x.Value)
            .Concat(fivegrams).ToDictionary(x => x.Key, x => x.Value);
        return result;
    }

    static Dictionary<string, List<string>> GetNgramsManyContinuation(List<List<string>> text, int n)
    {
        var ngrams = new Dictionary<string,List<string>>();
        var continuations = new List<string>();

        foreach (var words in text)
        {
            for (int i = 0; i < words.Count - n + 1; i++)
            {
                string ngramKey = MakeNgramKey(n, words, i);

                ngrams.TryAdd(ngramKey, continuations = new List<string> { });
                ngrams[ngramKey].Add(words[i + n - 1]);
            }
        }
        return ngrams;
    }

    static Dictionary<string, string> GetNgramsOnlyContinuation(List<List<string>> text, int n)
    {
        var ngrams = new Dictionary<string, string>();
        var frequency = new Dictionary<string, int>();

        foreach (var words in text)
        {
            for (int i = 0; i < words.Count - n + 1; i++)
            {
                string ngramKey = MakeNgramKey(n, words, i), ngram = $"{ngramKey} {words[i + n - 1]}";
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

    static string MakeNgramKey(int n, List<string> words, int i)
    {
        var ngramKey = new StringBuilder( $"{words[i]}" );
        for (int j =  1; j < n - 1; j++)
        {
            ngramKey.Append($" {words[i+j]}");
        }
        return ngramKey.ToString();
    }
}
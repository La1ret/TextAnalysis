using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Collections.Generic;
using System.Text;

namespace TextAnalysis;

static class FrequencyAnalysisTask
{
    public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
    {
        var result = new Dictionary<string, string>();
        var frequency = new Dictionary<string, int>();
        foreach (var words in text)
        {
            for (int i = 0; i < words.Count - 1; i++)//биграммы
            {
                var bigram = $"{words[i]} {words[i + 1]}";
                frequency.TryAdd(bigram, 0);
                frequency[bigram] += 1;

                result.TryAdd(words[i], words[i + 1]);
                frequency.TryAdd($"{words[i]} {result[words[i]]}", 1);

                if (frequency[bigram] > frequency[$"{words[i]} {result[words[i]]}"] || 
                    (frequency[bigram] == frequency[$"{words[i]} {result[words[i]]}"] && string.Compare(words[i + 1], result[words[i]]) == -1))
                    result[words[i]] = words[i + 1];
            }

            for (int i = 0; i < words.Count - 2; i++)//триграммы
            {
                string bigramKey = $"{words[i]} {words[i + 1]}", trigram = $"{bigramKey} {words[i + 2]}";
                frequency.TryAdd(trigram, 0);
                frequency[trigram] += 1;

                result.TryAdd(bigramKey, words[i+2]);
                frequency.TryAdd($"{bigramKey} {result[bigramKey]}", 1);

                if (frequency[trigram] > frequency[$"{bigramKey} {result[bigramKey]}"] || 
                    (frequency[trigram] == frequency[$"{bigramKey} {result[bigramKey]}"] && string.Compare(words[i + 2], result[bigramKey]) == -1))
                    result[bigramKey] = words[i + 2];
            }
        }
        return result;
    }

    static Dictionary<string, string> GetNgramms(List<List<string>> text, int n)
    {
        var ngramms = new Dictionary<string, string>();
        var frequency = new Dictionary<string, int>();

        foreach (var words in text)
        {
            for (int i = 0; i < words.Count - n + 1; i++)
            {
                string ngrammKey = GetNgrammKey(n, words, i), ngramm = $"{ngrammKey} {words[i + n - 1]}";
                frequency.TryAdd(ngramm, 0);
                frequency[ngramm] += 1;

                ngramms.TryAdd(ngrammKey, words[i + n - 1]);
                frequency.TryAdd($"{ngrammKey} {ngramms[ngrammKey]}", 1);

                if (frequency[ngramm] > frequency[$"{ngrammKey} {ngramms[ngrammKey]}"] ||
                    (frequency[ngramm] == frequency[$"{ngrammKey} {ngramms[ngrammKey]}"] && string.Compare(words[i + n - 1], ngramms[ngrammKey]) == -1))
                    ngramms[ngrammKey] = words[i + n - 1];
            }
        }
        return ngramms;
    }

    static string GetNgrammKey(int n, List<string> words, int i)
    {
        var ngrammKey = new StringBuilder( $"{words[i]}" );
        for (int j =  1; j < n; j++)
        {
            ngrammKey.Append($" {words[i+j]}");
        }
        return ngrammKey.ToString();
    }
}
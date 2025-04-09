using System.Text;

namespace TextAnalysis;

static class TextGeneratorTask
{
    public static string ContinuePhrase(
        Dictionary<string, string> nextWords,
        string phraseBeginning,
        int wordsCount)
    {
        var finalPhraseList = new List<string>();
        if (phraseBeginning != null)
        {
            foreach (var wordsFirstPhrase in phraseBeginning.Split(' '))
                finalPhraseList.Add(wordsFirstPhrase);
            for (int i = 0; i < wordsCount; i++)
            {
                if (TryUseNgram(nextWords, 5, ref finalPhraseList)) continue;
                if (TryUseNgram(nextWords, 4, ref finalPhraseList)) continue;
                if (TryUseNgram(nextWords, 3, ref finalPhraseList)) continue;
                if (TryUseNgram(nextWords, 2, ref finalPhraseList)) continue;
                break;
            }
        }
        var finalPhrase = new StringBuilder();
        for (int i = 0; i < finalPhraseList.Count; i++)
        {
            if (i == finalPhraseList.Count - 1) finalPhrase.Append($"{finalPhraseList[i]}");
            else finalPhrase.Append($"{finalPhraseList[i]} ");
        }
        return finalPhrase.ToString();
    }
    
    /// ////////////////////////////////////////////////////////////////////////

    public static string ContinuePhraseRandomNextWords(Dictionary<string, List<string>> nextWords,
                                                        string phraseBeginning,int wordsCount)
    {
        var finalPhraseList = new List<string>();
        var random = new Random();
        if (phraseBeginning != null)
        {
            foreach (var wordsFirstPhrase in phraseBeginning.Split(' '))
                finalPhraseList.Add(wordsFirstPhrase);
            for (int i = 0; i < wordsCount; i++)
            {
                if (TryUseRandomContinuationNgram(nextWords, 5, ref finalPhraseList)) continue;
                if (TryUseRandomContinuationNgram(nextWords, 4, ref finalPhraseList)) continue;
                if (TryUseRandomContinuationNgram(nextWords, 3, ref finalPhraseList)) continue;
                if (TryUseRandomContinuationNgram(nextWords, 2, ref finalPhraseList)) continue;
                break;
            }
        }
        var finalPhrase = new StringBuilder();
        for (int i = 0; i < finalPhraseList.Count; i++)
        {
            if (i == finalPhraseList.Count - 1) finalPhrase.Append($"{finalPhraseList[i]}");
            else finalPhrase.Append($"{finalPhraseList[i]} ");
        }
        return finalPhrase.ToString();
    }

    static bool TryUseRandomContinuationNgram(Dictionary<string, List<string>> ngram, int n, ref List<string> finalPhraseList)
    {
        if (finalPhraseList.Count >= n - 1)
        {
            string ngramKey = GetNgramKey(n, finalPhraseList);
            if (ngram.ContainsKey($"{ngramKey}"))
            {
                var rnd = new Random();
                var rndIndexInRange = rnd.Next(0, ngram[$"{ngramKey}"].Count);
                finalPhraseList.Add($"{ngram[$"{ngramKey}"][rndIndexInRange]}");
                return true;
            }
        }
        return false;
    }

    /// ////////////////////////////////////////////////////////////


    static bool TryUseNgram(Dictionary<string, string> ngram, int n, ref List<string> finalPhraseList)
    {
        if (finalPhraseList.Count >= n - 1)
        {
            string ngramKey = GetNgramKey(n, finalPhraseList);
            if (ngram.ContainsKey($"{ngramKey}"))
            {
                finalPhraseList.Add($"{ngram[$"{ngramKey}"]}");
                return true;
            }
        }
        return false; 
    }

    static string GetNgramKey(int n, List<string> finalPhraseList)
    {
        var ngramKey = new StringBuilder($"{finalPhraseList[^(n - 1)]}");
        for (int j = n - 2; j > 0; j--)
            ngramKey.Append($" {finalPhraseList[^j]}");

        return ngramKey.ToString();
    }
}
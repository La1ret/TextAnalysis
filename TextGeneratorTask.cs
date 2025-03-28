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
                if (finalPhraseList.Count >= 2 && nextWords.ContainsKey($"{finalPhraseList[finalPhraseList.Count - 2]} {finalPhraseList[finalPhraseList.Count - 1]}"))
                    finalPhraseList.Add($"{nextWords[$"{finalPhraseList[finalPhraseList.Count - 2]} {finalPhraseList[finalPhraseList.Count - 1]}"]}");

                else if (finalPhraseList.Count > 0 && nextWords.ContainsKey($"{finalPhraseList[finalPhraseList.Count - 1]}"))
                    finalPhraseList.Add($"{nextWords[$"{finalPhraseList[finalPhraseList.Count - 1]}"]}");

                else break;
            }
        }
        var finalPhrase = new StringBuilder();
        for (int i = 0; i < finalPhraseList.Count; i++)
        {
            if (i == finalPhraseList.Count - 1) finalPhrase.Append($"{finalPhraseList[i]}");
            else finalPhrase.Append($"{finalPhraseList[i]} ");
        }
        return finalPhrase.ToString();//phraseBeginning;
    }
}
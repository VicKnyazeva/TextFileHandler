using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace TextFileHandler
{
    struct ProcessingResult
    {
        public bool Success;
        public string Message;
    }

    class TextFileProcessor
    {
        public bool removePunctuation = false;
        public bool removeEmptyLines = false;
        public int minWordLength = 0;

        public ProcessingResult ProcessFile(string inputFilePath, string outputFilePath)
        {
            try
            {
                return InternalProcessFile(inputFilePath, outputFilePath);
            }
            catch (Exception ex)
            {
                return new ProcessingResult { Success = false, Message = ex.Message };
            }
        }

        private ProcessingResult InternalProcessFile(string inputFilePath, string outputFilePath)
        {
            if (!File.Exists(inputFilePath))
                return new ProcessingResult { Success = false, Message = "Файл не найден" };

            StringBuilder sbCurrentWord = new StringBuilder();
            StringBuilder sbFinal = new StringBuilder();

            char prevChar = '\0';

            using (StreamReader sReader = new StreamReader(inputFilePath))
            {
                int wordsInLine = 0;
                while (sReader.Peek() != -1)
                {
                    char ch = (char)sReader.Read();

                    prevChar = ch;

                    bool punctuation = char.IsPunctuation(ch);

                    if (!punctuation && !char.IsWhiteSpace(ch) && ch != '\n' && ch != '\r')
                    {
                        sbCurrentWord.Append(ch);
                    }
                    else
                    {
                        // Punctuation || Space || ch != '\n' || ch != '\r'-- граница слова
                        //
                        if (sbCurrentWord.Length > 0)
                        {
                            // слово закончилось на знаке препинания и т.п.
                            if (processWord(sbFinal, sbCurrentWord, wordsInLine == 0))
                                ++wordsInLine;
                        }
                        if (ch == '\r')
                            continue;

                        if (ch == '\n')
                        {
                            if (!removeEmptyLines || wordsInLine > 0)
                            {
                                sbFinal.Append("\n");
                            }
                            wordsInLine = 0;
                        }
                        else if (punctuation && !this.removePunctuation)
                        {
                            sbFinal.Append(ch);
                        }
                    }
                }

                if (sbCurrentWord.Length > 0)
                {
                    // слово закончилось на конце файла
                    processWord(sbFinal, sbCurrentWord, wordsInLine == 0);
                }

                using (var f = File.CreateText(outputFilePath))
                {
                    f.Write(sbFinal.ToString());
                }

                return new ProcessingResult { Success = true, Message = "Файл успешно обработан" };
            }
        }

        private bool processWord(StringBuilder sbFinal, StringBuilder sbCurrentWord, bool firstInLine)
        {
            Debug.Assert(sbCurrentWord.Length > 0);

            bool wordAdded = false;
            if (sbCurrentWord.Length > minWordLength)
            {
                if (!firstInLine)
                    sbFinal.Append(" ");

                sbFinal.Append(sbCurrentWord); //текущее накопленное слово переносим в Final

                wordAdded = true;
            }
            sbCurrentWord.Clear(); //чистим текущее слово
            return wordAdded;
        }
    }
}

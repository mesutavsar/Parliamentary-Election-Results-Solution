using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParliamentaryElectionResultsSolution
{
    public class ProvienceData
    {
        #region Main Function
        public static void Output()
        {
            List<double> provienceDataList = new List<double>();

            using (StreamReader filePathData = new StreamReader("C:\\Users\\mesut\\OneDrive\\Masaüstü\\Akademi\\" + //using kod bloğunu kullan
                                                                                                                    //ve sil gitsin bellekte yer kalmasın
                "10. Hafta 14.10.2023 Oop + txt Dosyasından Bilgi Alma\\10.Hafta_Ödev\\secim.txt"))
            {
                string row;
                bool skipFirstRow = true;

                while ((row = filePathData.ReadLine()) != null)
                {
                    if (skipFirstRow)
                    {
                        skipFirstRow = false;
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(row))
                    {
                        continue;
                    }

                    if (double.TryParse(row, out double data))
                    {
                        provienceDataList.Add(data); // Sayıyı listeye ekleyin
                    }

                    using (StreamWriter outputFile = new StreamWriter("outputProvienceDataList.txt"))
                    {
                        foreach (double data2 in provienceDataList)
                        {
                            outputFile.WriteLine(data2.ToString()); // Her sayıyı yeni dosyaya yaz
                        }
                    }

                    // Console.WriteLine(row); // Her satırı yazdır
                }
            }


            List<List<double>> provienceVoteList = new List<List<double>>();
            List<double> provienceVoteDataList = new List<double>();
            int lineCount = 0;
            string provincialPlateCode = null;
            double parliamentaryQuota = 0.0;

            using (StreamReader filePathData2 = new StreamReader("outputProvienceDataList.txt"))
            {
                string row;

                while ((row = filePathData2.ReadLine()) != null)
                {
                    if (double.TryParse(row, out double VoteAmount))
                    {
                        provienceVoteDataList.Add(VoteAmount); // Sadece oy miktarlarını ekle
                    }

                    lineCount++;

                    if (lineCount == 1)
                    {
                        provincialPlateCode = row; // 1. satır il plaka kodu (string olarak sakla)
                    }
                    else if (lineCount == 2)
                    {
                        parliamentaryQuota = double.Parse(row); // 2. satır milletvekili kontenjanı
                    }

                    if (lineCount == 10)
                    {
                        provienceVoteList.Add(new List<double>(provienceVoteDataList));
                        provienceVoteDataList.Clear();
                        lineCount = 0; // Bir sonraki ilin ilk satırı
                    }
                }
            }

            List<List<double>> numberOfDeputy = Election(provienceVoteList);

            for (int i = 0; i < provienceVoteList.Count; i++)
            {
                double provincialPlateCode2 = provienceVoteList[i][0];
                double parliamentaryQuota2 = provienceVoteList[i][1];
                double totalVoteAmount = 0.00;

                for (int j = 2; j < provienceVoteList[i].Count; j++)
                {
                    totalVoteAmount += provienceVoteList[i][j];
                }

                Console.WriteLine("\nİl Plaka Kodu: " + provincialPlateCode2);
                Console.WriteLine("Milletvekili Kontenjanı: " + parliamentaryQuota2);
                Console.WriteLine("Geçerli Oy Sayısı: " + totalVoteAmount);
                Console.WriteLine("------------------------");
                Console.WriteLine("Pusula Sırası" + "\tOy Sayısı" + "\tOy Yüzdesi" + "\tMV Sayısı ");

                int index = 2;
                int index2 = 0;
                for (int y = 0; y < 8; y++)
                {

                    Console.WriteLine((y + 1) + ". Parti\t" + provienceVoteList[i][index] + "\t\t" + Percentage(provienceVoteList[i][index], totalVoteAmount) + "\t    " + numberOfDeputy[i][index2]);
                    index++;
                    index2++;
                }
            }

            List<double> totalVotes = TurkeyElection(provienceVoteList);
            double total = 0;
            foreach (double vote in totalVotes)
            {
                total += vote;
            }

            List<List<double>> totalDeputy = Election(provienceVoteList);
            List<double> debutyByParty = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int c = 0; c < 8; c++)
            {
                double total2 = 0;
                for (int y = 0; y < 3; y++)
                {
                    total2 += totalDeputy[y][c];
                }
                debutyByParty[c] = total2;
            }

            double total5 = 0;
            for (int m = 0; m < 8; m++)
            {
                total5 += debutyByParty[m];
            }

            List<List<double>> totalProvience = Election(provienceVoteList);
            List<double> provienceByParty = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0 };


            for (int f = 0; f < 8; f++)
            {
                double total3 = 0;
                for (int h = 0; h < 3; h++)
                {
                    if (totalProvience[h][f] != 0)
                    {
                        total3 += 1;
                    }

                }
                provienceByParty[f] = total3;
            }

            Console.WriteLine();
            Console.WriteLine("------------------------------------------------------------------------------\n");
            Console.WriteLine("Türkiye Geneli");
            Console.WriteLine("Milletvekili Kontenjanı : " + total5);
            Console.WriteLine("Geçerli Oy Sayısı : " + total + "\n");
            Console.WriteLine("Pusula Sırası" + "\tOy Sayısı" + "\tOy Yüzdesi" + "\tMV Sayısı " + "\t0 İl MV Sayısı ");
            int index3 = 2;
            int index4 = 0;

            for (int z = 0; z < 8; z++)
            {
                Console.WriteLine((z + 1) + ". Parti" + "\t" + totalVotes[z] + "\t\t" + Percentage(totalVotes[z], total) + "\t      " + debutyByParty[z] + "\t\t   " + (3 - provienceByParty[z]));
                index3++;
                index4++;
            }

        }
        #endregion

        #region Percentage Calculation
        public static string Percentage(double num1, double num2)
        {
            double result = num1 * (100 / num2);
            string formattedResult = result.ToString("F2").PadLeft(8);
            return formattedResult;
        }
        #endregion

        #region Deputy Election 
        public static List<List<double>> Election(List<List<double>> provienceVoteList)
        {
            List<List<double>> info = new List<List<double>>();
            List<List<double>> numberOfDep = new List<List<double>>();
            double partyNumber = 1;

            foreach (var subList in provienceVoteList)
            {
                List<double> copySubList = new List<double>(subList);
                info.Add(copySubList);

            }

            for (int i = 0; i < info.Count; i++)
            {
                int deputyNumber = 0;
                List<double> list = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0 };
                for (int j = 0; j < info[i][1]; j++)
                {
                    double max = info[i].Max();
                    int index = info[i].IndexOf(max);
                    info[i][index] /= 2;
                    if (list[index - 2] == 0)
                    {
                        list[index - 2] = 1;
                    }
                    else
                    {
                        list[index - 2]++;
                    }
                }
                numberOfDep.Add(list);
            }
            return numberOfDep;
        }

        #endregion

        #region Turkey Election
        public static List<double> TurkeyElection(List<List<double>> provienceVoteList)
        {
            List<double> list2 = new List<double> { 0, 0, 0, 0, 0, 0, 0, 0 };
            int y = 2;

            for (int i = 0; i < 8; i++)
            {
                double total = 0;
                for (int k = 0; k < provienceVoteList.Count; k++)
                {
                    total += provienceVoteList[k][y];
                }
                y++;
                list2[i] = total;
            }
            return list2;
        }
        #endregion

    }
}

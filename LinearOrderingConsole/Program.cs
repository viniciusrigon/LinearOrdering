using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinearOrdering.Parser;
using LinearOrdering.GRASP;
using System.Globalization;

namespace LinearOrdering
{
    class Program
    {
        
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("Linha de comando correta é: LinearOrderingConsole [nome-do-arquivo] [numero-maximo-iteracoes] [alfa]");
                }
                else if (args.Length == 3)
                {
                    string nomeArquivo = string.Empty;
                    if (!String.IsNullOrEmpty(args[0]))
                    {
                        nomeArquivo = args[0];
                    }

                    int numeroIteracoes = Convert.ToInt32(args[1]);
                    decimal alfa = Convert.ToDecimal(args[2], new CultureInfo("en-US"));

                    LinearOrdering.Parser.Parser parser = new LinearOrdering.Parser.Parser(nomeArquivo);
                    parser.InputLinearOrderingFile();
                    

                    GRASP.GRASP grasp = new GRASP.GRASP();
                    
                    grasp.DadosProblema = parser.parametrosProblema;

                    DateTime tempoInicial = DateTime.Now;
                    
                    Solucao sol = grasp.grasp(numeroIteracoes, alfa);
                    DateTime tempoFinal = DateTime.Now;
                    Console.WriteLine("Sequencia de permutações: ");
                    Console.WriteLine();
                    foreach (var item in sol.Permutacoes)
                    {
                        Console.Write("{0} ", item.ToString());
                        
                    }

                    Console.WriteLine("Instancia: {0}", nomeArquivo);
                    Console.WriteLine("Solução inicial: {0}", sol.ValorSolucaoInicial);
                    Console.WriteLine("Solucao encontrada: {0} ", sol.ValorSolucao);
                    Console.WriteLine("Alfa: {0} ", alfa);
                    Console.WriteLine("Numero interacoes: {0} ", numeroIteracoes);
                    Console.WriteLine("Tempo de execução(em segundos): {0} ", (tempoFinal.Subtract(tempoInicial)).TotalSeconds);

                }
                else
                {
                    Console.WriteLine("Linha de comando correta é: AirLandingConsole [nome-do-arquivo] [numero-maximo-iteracoes] [alfa]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

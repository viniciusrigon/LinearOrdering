using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using LinearOrdering.Parser;
using System.Security.Cryptography;
using System.Timers;

namespace LinearOrdering.GRASP
{   
    public class Arco
    {
        public int nodoInicial { get; set; }
        public int nodoFinal { get; set; }
        public int valor { get; set; }
    }

    public class Solucao
    {
        private List<Arco> arcos;
        public List<Arco> Arcos
        {
            get;
            set;
        }

        private List<int> permutacoes;
        public List<int> Permutacoes
        {
            get { return permutacoes; }
            set { permutacoes = value; }
        }
                
        private decimal valorSolucao;
        public decimal ValorSolucao
        {
            get { return valorSolucao; }
            set { valorSolucao = value; }
        }

        private decimal valorSolucaoInicial;
        public decimal ValorSolucaoInicial
        {
            get { return valorSolucaoInicial; }
            set { valorSolucaoInicial = value; }
        }

        public Solucao()
        {           
            Permutacoes = new List<int>();
            Arcos = new List<Arco>();
        }
        
    }

    public class GRASP
    {
        
        private Dados dadosProblema;       
        public Dados DadosProblema
        {
            get { return dadosProblema; }
            set { dadosProblema = value; }
        }

        public GRASP()
        {
            this.dadosProblema = new Dados();
        }
        
        
        public Solucao grasp(int numIter, decimal pAlfa)
        {   
            Solucao solucaoInicial = new Solucao();
            solucaoInicial.ValorSolucao = decimal.MinValue;

            Solucao solucao = new Solucao();
            solucaoInicial.ValorSolucao = decimal.MinValue;
            
            bool PrimeiraSolucao = true;
           
            int num_iter = 0;

            while (num_iter <= numIter)
            {
                Console.WriteLine("Iteracao : {0}", num_iter);

                DateTime tempoInicial = DateTime.Now;
                solucaoInicial = greedyRandomizedSolution(pAlfa);
                DateTime tempoFinal = DateTime.Now;
                Console.WriteLine("Tempo de execução gulosidade - randomica(em segundos): {0} ", (tempoFinal.Subtract(tempoInicial)).TotalSeconds);
                Console.WriteLine();
                tempoInicial = DateTime.Now;
                solucaoInicial = buscaLocal(numIter, solucaoInicial);
                tempoFinal = DateTime.Now;
                Console.WriteLine("Tempo de execução busca local(em segundos): {0} ", (tempoFinal.Subtract(tempoInicial)).TotalSeconds);
                Console.WriteLine();

                if (PrimeiraSolucao)
                {
                    solucao.ValorSolucaoInicial = solucaoInicial.ValorSolucao;
                    PrimeiraSolucao = false;
                }

                if (solucaoInicial.ValorSolucao > solucao.ValorSolucao)
                {
                    Console.WriteLine("Solução atualizada: {0}", solucaoInicial.ValorSolucao);
                    Console.WriteLine();
                    
                    solucao.ValorSolucao = solucaoInicial.ValorSolucao;
                    solucao.Permutacoes = solucaoInicial.Permutacoes;
                    solucao.Arcos = solucaoInicial.Arcos;
                }

                num_iter++;
            }

            

            return solucao;

        }

        private Solucao greedyRandomizedSolution(decimal alfa)
        {
            Solucao sol = new Solucao();
            Random rand = new Random();
            sol.ValorSolucao = 0;
            List<int> nodosInseridos = new List<int>();
            
            while (sol.Permutacoes.Count < DadosProblema.numNodos)
            {
                List<Arco> listaArcos = new List<Arco>();

                for (int i = 0; i < DadosProblema.numNodos; i++)
                {
                    for (int j = i; j < DadosProblema.numNodos; j++)
                    {
                        if (j > i)
                        {
                            Arco arco = new Arco();

                            int pesoArco = DadosProblema.matrizCusto[i, j];

                            arco.nodoInicial = i;
                            arco.nodoFinal = j;
                            arco.valor = pesoArco;

                            listaArcos.Add(arco);
                        }
                    }
                }

                List<Arco> ListaRestrita = criarLRC(alfa, listaArcos);
                
                Arco arcoRandomico = ListaRestrita[rand.Next(0, ListaRestrita.Count)];

                Arco arcoJaInserido = sol.Arcos.Find(x => x.nodoInicial == arcoRandomico.nodoInicial &&
                    x.nodoFinal == arcoRandomico.nodoFinal);


                if (arcoJaInserido == null)
                {
                    sol.Arcos.Add(arcoRandomico);

                    int nodoInicial = arcoRandomico.nodoInicial;
                    int nodoFinal = arcoRandomico.nodoFinal;

                    if (!sol.Permutacoes.Contains(nodoInicial))
                        sol.Permutacoes.Add(nodoInicial);
                    if (!sol.Permutacoes.Contains(nodoFinal))
                        sol.Permutacoes.Add(nodoFinal);

                }

            }

            CalcularValorSolucao(sol);

            return sol;

        }

        private Solucao buscaLocal(int numeroIter, Solucao sol)
        {
            //Repete
            //pegar a solução(S), trocar de posição 2 permutações

            /*calcular o valor da nova solução(S'),
                     }   se S > S' 
                      S=S'
                senão
                  descarta S'
                até S nao melhorar mais*/
            
            int no_update = 0;

            Random rand = new Random();

            do
            {
                Solucao novaSolucao = new Solucao();
                novaSolucao = copiaSolucao(sol);

                int posicao1 = rand.Next(0, sol.Permutacoes.Count);
                int posicao2 = rand.Next(0, sol.Permutacoes.Count);

                int nodo1 = sol.Permutacoes[posicao1];
                int nodo2 = sol.Permutacoes[posicao2];

                novaSolucao.Permutacoes.RemoveAt(posicao1);
                novaSolucao.Permutacoes.Insert(posicao1, nodo2);

                novaSolucao.Permutacoes.RemoveAt(posicao2);
                novaSolucao.Permutacoes.Insert(posicao2, nodo1);


                CalcularValorSolucao(novaSolucao);


                if (novaSolucao.ValorSolucao > sol.ValorSolucao)
                {
                    sol.Permutacoes = novaSolucao.Permutacoes;
                    sol.Arcos = novaSolucao.Arcos;
                    break;
                    //no_update = 0;
                }
                else
                {
                    no_update++;
                }

            } while (no_update < numeroIter);

            return sol;
           
        }

        private static List<Arco> criarLRC(decimal alfa, List<Arco> listaCandidatos)
        {
            listaCandidatos.Sort(delegate(Arco um, Arco dois) { return um.valor.CompareTo(dois.valor); });

            List<Arco> listaRestritaCandidatos = new List<Arco>();
           
            listaCandidatos.ForEach(delegate(Arco arc) 
            {
                if (listaRestritaCandidatos.Count < ((listaCandidatos.Count * alfa) + 1))
                {
                    listaRestritaCandidatos.Add(arc);
                }
            
            });
            
            return listaRestritaCandidatos;
        }

        private void CalcularValorSolucao(Solucao sol)
        {
            sol.ValorSolucao = 0;
            int[,] matrizPermutada = new int[DadosProblema.numNodos, DadosProblema.numNodos];
            for (int i = 0; i < sol.Permutacoes.Count; i++)
            {
                for (int j = 0; j < sol.Permutacoes.Count; j++)
                {
                    int itr1 = sol.Permutacoes[i];
                    int itr2 = sol.Permutacoes[j];

                    matrizPermutada[i, j] = DadosProblema.matrizCusto[itr1, itr2];
                }
            }

            for (int i = 0; i < sol.Permutacoes.Count; i++)
            {
                for (int j = 0; j < sol.Permutacoes.Count; j++)
                {
                    if (j > i)
                        sol.ValorSolucao += matrizPermutada[i, j];
                }
            }
        }

        private Solucao copiaSolucao(Solucao sol)
        {
            Solucao solCopia = new Solucao();

            foreach (int item in sol.Permutacoes)
            {
                solCopia.Permutacoes.Add(item);
            }

            foreach (Arco arco in sol.Arcos)
            {
                solCopia.Arcos.Add(arco);
            }

            solCopia.ValorSolucao = sol.ValorSolucao;
           
            return solCopia;
        }
        

        
    }
}

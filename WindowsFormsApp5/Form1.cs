/* *******************************************************************
* Colegio Técnico Antônio Teixeira Fernandes (Univap)
* Curso Técnico em Informática - Data de Entrega: 01/09/2023
* Autores do Projeto: Gabriel Costa Fileno
*                     Rafael Augusto Guimarães da Silva
* Turma: 2H
* Atividade Proposta em aula
* Observação: <colocar se houver>
* 
* problema_aula.cs
* 
* Funcionalidades dos componentes:
*   textBox1 -> Caixa de texto para a entrada do preço.
*   textBox2 -> Caixa de texto para a entrada do número de parcelas.
*   textBox3 -> Caixa de texto multilinha para envio dos resultados.
*   button1 -> Botão para a execução da rotina de compra.
*   button2 -> Botão para a execução da rotina de pagamento.
*   dateTimePicker1 -> Seletor de data para a entrada do dia do pagamento.
*   label1 -> Rótulo para informar o que deve ser inserido na textbox correspondente.
*   label2 -> Rótulo para informar o que deve ser inserido na textbox correspondente.
*   label3 -> Rótulo para informar o que deve ser inserido na textbox correspondente.
*   label4 -> Rótulo para a saída do valor total a ser pago.
*   label5 -> Rótulo para a saída do valor da última parcela, com reajuste de juros se nescessário.
* ************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        decimal valor_total = 0;
        int numero_parcelas = 0;
        int parcelas_pagas = 0;
        DateTime data_compra;
        decimal valor_compra;
        decimal diferenca = 0;
        decimal valor_parcelas;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
            parcelas_pagas = 0;
            data_compra = DateTime.Now;
            data_compra = new DateTime(data_compra.Year, data_compra.Month, data_compra.Day, 0, 0, 0);
            numero_parcelas = int.Parse(textBox2.Text);
            valor_compra = Math.Round(decimal.Parse(textBox1.Text), 2);
            valor_parcelas = valor_compra / numero_parcelas;
            valor_parcelas = Math.Truncate(valor_parcelas * 100) / 100;
            diferenca = (valor_compra - (valor_parcelas * numero_parcelas))*100;
            
            for (int i = 1; i <= numero_parcelas; i++)
            {
                DateTime data_parcela = data_compra.AddMonths(i);
                int dia = (int) data_parcela.DayOfWeek;
                if (dia == 0)
                    data_parcela = data_parcela.AddDays(1);
                else if (dia == 6)
                    data_parcela = data_parcela.AddDays(2);
                string infos_parcela;
                if (i <= diferenca)
                {
                    decimal valor_parcelas_reajuste = valor_parcelas + 0.01M;
                    infos_parcela = i.ToString() + "ª Parcela - Vencimento: " + data_parcela.ToString("dd/MM/yy") + " - Valor: R$" + valor_parcelas_reajuste.ToString("0.00") + Environment.NewLine;
                }
                else
                {
                    infos_parcela = i.ToString() + "ª Parcela - Vencimento: " + data_parcela.ToString("dd/MM/yy") + " - Valor: R$" + valor_parcelas.ToString("0.00") + Environment.NewLine;
                }

                textBox3.AppendText(infos_parcela);
            }
            valor_total = valor_compra;
            label4.Text = "VALOR TOTAL:" + Environment.NewLine + "R$" + valor_total.ToString("0.00"); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime data_pagamento = dateTimePicker1.Value;
            data_pagamento = new DateTime(data_pagamento.Year,data_pagamento.Month, data_pagamento.Day, 0, 0, 0);
            if (parcelas_pagas != numero_parcelas && data_pagamento >= data_compra)
            {
                decimal valor_parcelas_reajuste = 0;
                textBox3.Text = "";
                parcelas_pagas++;

                string infos_pagamento;
                for (int i = parcelas_pagas; i <= numero_parcelas; i++)
                {
                     DateTime data_parcela = data_compra.AddMonths(i);
                     int dia = (int) data_parcela.DayOfWeek;
                     if (dia == 0)
                        data_parcela = data_parcela.AddDays(1);
                     else if (dia == 6)
                        data_parcela = data_parcela.AddDays(2);

                    if (i <= diferenca)
                       valor_parcelas_reajuste = valor_parcelas + 0.01M;
                    else
                       valor_parcelas_reajuste = valor_parcelas;                   

                     if (i == parcelas_pagas)
                     {
                        valor_total -= valor_parcelas_reajuste;
               
                        if (data_pagamento <= data_parcela)
                        {
                            infos_pagamento = "Ultima parcela paga: R$" + valor_parcelas_reajuste.ToString("0.00") + Environment.NewLine + "Sem juros.";
                            label5.Text = infos_pagamento;
                        }
                        else
                        {
                            decimal parcela_juros = valor_parcelas_reajuste * 1.03M;
                            infos_pagamento = "Ultima parcela paga: R$" + parcela_juros.ToString("0.00") + Environment.NewLine + "Parcela em atraso, juros de 3%.";
                            label5.Text = infos_pagamento;
                        }
                     }
                     else
                     {
                        string infos_parcela = i.ToString() + "ª Parcela - Vencimento: " + data_parcela.ToString("dd/MM/yy") + " - Valor: R$" + valor_parcelas_reajuste.ToString("0.00") + Environment.NewLine;
                        textBox3.AppendText(infos_parcela);
                     }
                        
                }
                label4.Text = "VALOR TOTAL:" + Environment.NewLine + "R$" + valor_total.ToString("0.00");
            }
            else
            {
                label5.Text = "O pagamento é impossível de ser realizado.";
            }
        }
    }
}
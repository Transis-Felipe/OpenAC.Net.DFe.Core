// ***********************************************************************
// Assembly         : OpenAC.Net.DFe.Core
// Author           : RFTD
// Created          : 03-09-2018
//
// Last Modified By : RFTD
// Last Modified On : 03-09-2018
// ***********************************************************************
// <copyright file="ChaveDFe.cs" company="OpenAC .Net">
//		        		   The MIT License (MIT)
//	     		    Copyright (c) 2014-2022 Grupo OpenAC.Net
//
//	 Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//	 The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//	 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Text;
using System.Text.RegularExpressions;
using OpenAC.Net.Core;
using OpenAC.Net.Core.Extensions;
using OpenAC.Net.DFe.Core.Extensions;

namespace OpenAC.Net.DFe.Core.Common;

/// <summary>
///
/// </summary>
public sealed class ChaveDFe
{
    #region Constructors

    internal ChaveDFe(DFeCodUF ufEmitente, DateTime dataEmissao, string cnpjEmitente, int modelo, int serie,
        long numero, DFeTipoEmissao tipoEmissao, int cNumerico)
    {
        var chave = new StringBuilder();

        chave.Append(ufEmitente.GetDFeValue())
            .Append(dataEmissao.ToString("yyMM"))
            .Append(cnpjEmitente)
            .Append(modelo.ToString("D2"))
            .Append(serie.ToString("D3"))
            .Append(numero.ToString("D9"))
            .Append(tipoEmissao.GetDFeValue())
            .Append(cNumerico.ToString("D8"));

        var calcDigito = new CalcDigito
        {
            FormulaDigito = CalcDigFormula.Modulo11,
            Documento = chave.ToString(),
            MultiplicadorInicial = 2,
            MultiplicadorFinal = 9
        };

        calcDigito.Calcular();

        chave.Append(calcDigito.DigitoFinal);

        Chave = chave.ToString();
        Digito = calcDigito.DigitoFinal;
    }

    #endregion Constructors

    #region Properties

    public string Chave { get; }

    public int Digito { get; }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Gera a chave do documento fiscal
    /// </summary>
    /// <param name="ufEmitente">UF do emitente do DF-e</param>
    /// <param name="dataEmissao">Data de emiss�o do DF-e</param>
    /// <param name="cnpjEmitente">CNPJ do emitente do DF-e</param>
    /// <param name="modelo">Modelo do DF-e</param>
    /// <param name="serie">S�rie do DF-e</param>
    /// <param name="numero">Numero do DF-e</param>
    /// <param name="tipoEmissao">Tipo de emiss�o do DF-e. Informar inteiro conforme consta no manual de orienta��o do contribuinte para o DF-e</param>
    /// <param name="cNumerico">C�digo num�rico que comp�e a Chave de Acesso. N�mero gerado pelo emitente para cada DF-e</param>
    /// <returns>Retorna a chave DFe</returns>
    public static ChaveDFe Gerar(DFeCodUF ufEmitente, DateTime dataEmissao, string cnpjEmitente, int modelo, int serie,
        long numero, DFeTipoEmissao tipoEmissao, int cNumerico)
    {
        return new ChaveDFe(ufEmitente, dataEmissao, cnpjEmitente, modelo, serie, numero, tipoEmissao, cNumerico);
    }

    /// <summary>
    /// Informa se a chave de um DF-e � v�lida
    /// </summary>
    /// <param name="chave"></param>
    /// <returns></returns>
    public static bool Validar(string chave)
    {
        if (chave.IsEmpty()) return false;

        chave = chave.Trim();
        if (chave.Trim().Length != 44) return false;

        var digitoVerificador = chave.Substring(43, 1).ToInt32();

        var calcDigito = new CalcDigito
        {
            Documento = chave.Substring(0, 43)
        };

        calcDigito.CalculoPadrao();
        calcDigito.Calcular();

        return digitoVerificador == calcDigito.DigitoFinal;
    }

    /// <summary>
    /// Formata a chave do documento fiscal.
    /// </summary>
    /// <param name="chave"></param>
    /// <returns></returns>
    public static string Formatar(string chave)
    {
        return Regex.Replace(chave, ".{4}", "$0 ");
    }

    #endregion Methods
}
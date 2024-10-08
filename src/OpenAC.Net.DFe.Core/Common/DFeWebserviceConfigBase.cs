// ***********************************************************************
// Assembly         : OpenAC.Net.DFe.Core
// Author           : RFTD
// Created          : 01-31-2016
//
// Last Modified By : RFTD
// Last Modified On : 06-07-2016
// ***********************************************************************
// <copyright file="DFeWebserviceConfigBase.cs" company="OpenAC .Net">
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
using System.Net;
using OpenAC.Net.Core.Extensions;
using OpenAC.Net.DFe.Core.Extensions;

namespace OpenAC.Net.DFe.Core.Common;

public abstract class DFeWebserviceConfigBase
{
    #region Constructor

    /// <summary>
    /// Inicializa uma nova instancia da classe <see cref="DFeWebserviceConfigBase"/>.
    /// </summary>
    protected DFeWebserviceConfigBase()
    {
        Ambiente = DFeTipoAmbiente.Homologacao;
        AjustaAguardaConsultaRet = false;
        AguardarConsultaRet = 1;
        Tentativas = 3;
        IntervaloTentativas = 1000;

#if NETCORE
        Protocolos = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
#else
            Protocolos = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls |
                         SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

#endif
    }

    #endregion Constructor

    #region Properties

    /// <summary>
    /// Define/retorna se deve ou n�o salvar os arquivos soap.
    /// </summary>
    public bool Salvar { get; set; }

    /// <summary>
    /// Gets or sets the ambiente.
    /// </summary>
    /// <value>The ambiente.</value>
    public DFeTipoAmbiente Ambiente { get; set; }

    /// <summary>
    /// Retorna o c�digo do ambiente.
    /// </summary>
    /// <value>The ambiente codigo.</value>
    public int AmbienteCodigo => Ambiente.GetDFeValue().ToInt32();

    /// <summary>
    /// Gets or sets the tentativas.
    /// </summary>
    /// <value>The tentativas.</value>
    public int Tentativas { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int IntervaloTentativas { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [ajusta aguarda consulta ret].
    /// </summary>
    /// <value><c>true</c> if [ajusta aguarda consulta ret]; otherwise, <c>false</c>.</value>
    public bool AjustaAguardaConsultaRet { get; set; }

    /// <summary>
    /// Gets or sets the aguardar consulta ret.
    /// </summary>
    /// <value>The aguardar consulta ret.</value>
    public uint AguardarConsultaRet { get; set; }

    /// <summary>
    ///
    /// </summary>
    public SecurityProtocolType Protocolos { get; set; }

    /// <summary>
    ///
    /// </summary>
    public TimeSpan? TimeOut
    {
        get
        {
            TimeSpan? timeOut = null;
            if (AjustaAguardaConsultaRet)
                timeOut = TimeSpan.FromSeconds((int)AguardarConsultaRet);

            return timeOut;
        }
    }

    #endregion Properties
}
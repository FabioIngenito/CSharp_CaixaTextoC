using System;
using System.Globalization;
using System.Drawing;
using System.Windows.Forms;

namespace clsCaixaTexto
{
    /// <summary>
    /// Based on the text below, but added new functions:
    /// Baseado no texto abaixo, mas adicionadas novas funções:
    /// 
    /// How to: Control User Input in a Numeric Text Box
    /// https://msdn.microsoft.com/en-us/library/vstudio/dd183783(v=vs.90).aspx
    ///
    /// Como: Criar um controle de usuário de entrada em uma caixa de texto numérica
    /// https://msdn.microsoft.com/pt-br/library/ms229644(v=VS.90).aspx
    /// </summary>
    public class clsCaixaTexto : TextBox
    {
        #region "Atributos"

        /// <summary>
        /// Não = No, Sim = Yes
        /// </summary>
        public enum ListaOpcoes
        {
            Nao,        
            Sim
        }
        /// <summary>
        /// Normal = Normal, Numero = Number, Letra = Letter
        /// </summary>
        public enum Lista
        {
            Normal,
            Numero,
            Letra
        }
        /// <summary>
        /// Inicio = Start, Fim = End, Completo = Complete
        /// </summary>
        public enum CursorInicial
        {
            Inicio,
            Fim,
            Completo
        }
        private struct VariaveisP
        {
            public Boolean blnPermitirEspaco;
            public Boolean blnPermitirSinalNegativo;
            public String strSeparadorDecimal;
            public Color oleCorFundoDepois;
            public Color oleCorFundoDurante;
            public Color oleCorLetraDepois;
            public Color oleCorLetraDurante;
            public String strTexto;
            public ListaOpcoes oBloqueado;
            public Lista strTipo;
            public CursorInicial strPosicaoCursor;
        }

        VariaveisP Variaveis;

        private String strTentativaBloqueada;

        #endregion

        #region "Métodos"

        /// <summary>
        /// Shortcut to create a constructor, type: "ctor" + tab + tab
        /// Atalho para criar um construtor, digite: "ctor" + tab + tab
        /// This constructor "pre-configures" the inheritance to clsCaixatexto.
        /// Este construtor "pré-configura" a herança a clsCaixatexto.
        /// If you want the "boot" of a different, change below.
        /// Se você quiser que a caixa "inicialize" de uma fora diferente, mude abaixo.
        /// </summary>
        public clsCaixaTexto()
        {
            Variaveis.blnPermitirEspaco = true;
            Variaveis.blnPermitirSinalNegativo = true;
            BackColor = Color.LightCyan;
            Variaveis.oleCorFundoDurante = Color.LightYellow;
            Variaveis.oleCorFundoDepois = Color.PaleTurquoise;
            Variaveis.strSeparadorDecimal = ",";
        }

        #endregion

        #region "Eventos"

        /// <summary>
        /// Restricts the entry of characters to digits, the space, the negative sign, the decimal
        /// Restringe a entrada de caracteres para dígitos, o espaço, o sinal negativo, o ponto 
        /// point, and editing keys (rewind).
        /// decimal, e edição de teclas (retrocesso).
        /// Put the box in Uppercase (upper case) or lowercase (lower case) automatically.
        /// Coloca a Caixa em Maiúscula (Caixa Alta) ou Minúscula (Caixa Baixa) Automaticamente.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            string SeparadorDecimal = numberFormatInfo.NumberDecimalSeparator;
            string SeparadorGrupo = numberFormatInfo.NumberGroupSeparator;
            string SinalNegativo = numberFormatInfo.NegativeSign;
            string keyInput = e.KeyChar.ToString();
            strTentativaBloqueada = "";

            #region Bloqueado
            if (Variaveis.oBloqueado == ListaOpcoes.Sim)
            {
                e.Handled = true;
                strTentativaBloqueada = "Propriedade Bloqueado";
            }
            #endregion

            #region PermiteEspaco
            if (!(Variaveis.blnPermitirEspaco))
            {
                if (e.KeyChar == ' ')
                {
                    e.Handled = true;
                    strTentativaBloqueada = "Espaço Bloqueado";
                }
            }
            #endregion

            #region Numero Letra
            if (Variaveis.strTipo == Lista.Numero)
            {
                if (!(Char.IsNumber(e.KeyChar) ||
                    e.KeyChar == '-' ||
                    keyInput.Equals(Variaveis.strSeparadorDecimal) ||
                    e.KeyChar == '\b'))
                {
                    e.Handled = true;
                    strTentativaBloqueada = "Somente Números";
                }

                if (!(Variaveis.blnPermitirSinalNegativo))
                {
                    if (e.KeyChar == '-')
                    {
                        e.Handled = true;
                        strTentativaBloqueada = "Não Permite Negativo";
                    }
                }
            }
            else if (Variaveis.strTipo == Lista.Letra)
            {
                if (Char.IsNumber(e.KeyChar))
                {
                    e.Handled = true;
                    strTentativaBloqueada = "Somente Letras";
                }
            }
            #endregion
        }

        /// <summary>
        /// Positions the alignment of Letter, the background colors and the font into the box.
        /// Posiciona o Alinhamento da Letra, as cores de fundo e de letra ao entrar na caixa.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            base.BackColor = Variaveis.oleCorFundoDurante;
            base.ForeColor = Variaveis.oleCorLetraDurante;
        }

        /// <summary>
        /// Positions the alignment of Letter, background colors and fonts when you leave in the box.
        /// Posiciona o Alinhamento da Letra, as cores de fundo e de letra ao sair na caixa.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLeave(System.EventArgs e)
        {
            BackColor = Variaveis.oleCorFundoDepois;
            ForeColor = Variaveis.oleCorLetraDepois;
        }

        /// <summary>
        /// To receive the focus positions the cursor within the text box.
        /// Ao receber o foco posiciona o cursor dentro da caixa de texto.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            #region Posição Cursor

            if (Variaveis.strPosicaoCursor == CursorInicial.Inicio)
            {
                base.SelectionStart = 0;
            }
            else if (Variaveis.strPosicaoCursor == CursorInicial.Fim)
            {
                base.SelectionStart = base.TextLength;
            }
            else
            {
                base.SelectionStart = 0;
                base.SelectionLength = base.TextLength;
            }

            #endregion
        }

        /// <summary>
        /// ToolboxBitmapAttribute
        /// ToolboxBitmapAttribute Class
        /// https://msdn.microsoft.com/en-us/library/system.drawing.toolboxbitmapattribute(v=vs.110).aspx
        /// </summary>
        protected override void InitLayout()
        {
            base.InitLayout();
        }

        /// <summary>
        /// Returns an integer value
        /// Retorna um valor inteiro
        /// </summary>
        /// <returns>O valor inteiro</returns>
        public Int32 RetornaInteiro()
        {
                return ValorInteiro;
        }

        /// <summary>
        /// Checks if the passed text "is numeric?"
        /// Verifica se o texto passado "É Numérico?"
        /// </summary>
        /// <returns>Retorna: Verdadeiro ou False</returns>
        public Boolean ENumerico()
        {
            float output;
            return float.TryParse(this.Text, out output);
        }

        #endregion

        #region "Propriedades"

        /// <summary>
        /// Returns an integer value
        /// Retorna o valor inteiro.
        /// </summary>
        private Int32 ValorInteiro
        {
            get
            {
                if (ENumerico())
                    return Convert.ToInt32(this.ValorDecimal);
                else
                    return 0;
            }
        }

        /// <summary>
        /// Returns a decimal value
        /// Retorna o valor decimal.
        /// </summary>
        private Decimal ValorDecimal
        {
            get
            {
                if (ENumerico())
                    return Decimal.Parse(this.Text);
                else
                    return 0;
            }
        }

        /// <summary>
        /// Checks if space allows
        /// Verifica se permite espaço
        /// </summary>
        public Boolean PermitirEspaco
        {
            get { return this.Variaveis.blnPermitirEspaco; }
            set { this.Variaveis.blnPermitirEspaco = value; }
        }

        /// <summary>
        /// Check allowed negative sinal.
        /// Verifica se permite sinal negativo.
        /// </summary>
        public Boolean PermitirSinalNegativo
        {
            get { return this.Variaveis.blnPermitirSinalNegativo; }
            set { this.Variaveis.blnPermitirSinalNegativo = value; }
        }

        /// <summary>
        /// Checks the decimal separator.
        /// Verifica separador decimal.
        /// </summary>
        public String SeparadorDecimal
        {
            get { return this.Variaveis.strSeparadorDecimal; }
            set { this.Variaveis.strSeparadorDecimal = value; }
        }

        /// <summary>
        /// Background color During the Cursor
        /// Cor de Fundo Durante o Cursor
        /// </summary>
        public Color CorFundoDurante
        {
            get { return Variaveis.oleCorFundoDurante; }
            set { Variaveis.oleCorFundoDurante = value; }
        }

        /// <summary>
        /// Background Color After the Cursor
        /// Cor de Fundo Depois do Cursor
        /// </summary>
        public Color CorFundoDepois
        {
            get { return Variaveis.oleCorFundoDepois; }
            set { Variaveis.oleCorFundoDepois = value; }
        }

        /// <summary>
        /// Color the Letter During the Cursor
        /// Cor da Letra Durante o Cursor
        /// </summary>
        public Color CorLetraDurante
        {
            get { return Variaveis.oleCorLetraDurante; }
            set { Variaveis.oleCorLetraDurante = value; }
        }

        /// <summary>
        /// Color the letter After the Cursor
        /// Cor da Letra Depois o Cursor
        /// </summary>
        public Color CorLetraDepois
        {
            get { return Variaveis.oleCorLetraDepois; }
            set { Variaveis.oleCorLetraDepois = value; }
        }

        /// <summary>
        /// Stores Default Text
        /// Armazena Texto Padrão
        /// </summary>
        public String Texto
        {
            get { return Variaveis.strTexto; }
            set { Variaveis.strTexto = value; }
        }

        /// <summary>
        /// Block Check
        /// Verifica Bloqueio
        /// </summary>
        public ListaOpcoes Bloqueado
        {
            get { return Variaveis.oBloqueado; }
            set { Variaveis.oBloqueado = value; }
        }

        /// <summary>
        /// Type of Text: Normal, Number or letter.
        /// Tipo do Texto: Normal, Número ou Letra.
        /// </summary>
        public Lista Tipo
        {
            get { return Variaveis.strTipo; }
            set { Variaveis.strTipo = value; }
        }

        /// <summary>
        /// Cursor Position to receive focus.
        /// Posição do Cursor ao receber foco.
        /// </summary>
        public CursorInicial PosicaoCursor
        {
            get { return Variaveis.strPosicaoCursor; }
            set { Variaveis.strPosicaoCursor = value; }
        }

        /// <summary>
        /// Read-only Variable.
        /// Variável somente de leitura.
        /// Returns the type of lock, if blocked.
        /// Retorna o tipo de bloqueio, caso seja bloqueado.
        /// </summary>
        public String TentativaBloqueada
        {
            get { return strTentativaBloqueada; }
        }

        #endregion
    }
}
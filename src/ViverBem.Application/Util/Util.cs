using System;
using System.Collections.Generic;
using System.Text;

namespace ViverBem.Application.Util
{
    public static class Util
    {
        public static string RemoveCaracteresEspeciais(string texto, bool trim, bool acentos, bool pontuacao, bool quebralinha, bool espacamento)
        {
            if (texto == null)
            {
                return "";
            }

            if (texto.Length == 0)
            {
                return "";
            }

            #region Acentos
            if (acentos)
            {
                texto = texto.Replace("ã", "a");
                texto = texto.Replace('Ã', 'A');
                texto = texto.Replace('â', 'a');
                texto = texto.Replace('Â', 'A');
                texto = texto.Replace('á', 'a');
                texto = texto.Replace('Á', 'A');
                texto = texto.Replace('à', 'a');
                texto = texto.Replace('À', 'A');
                texto = texto.Replace("ä", "a");
                texto = texto.Replace("Ä", "A");
                texto = texto.Replace('ç', 'c');
                texto = texto.Replace('Ç', 'C');
                texto = texto.Replace('é', 'e');
                texto = texto.Replace('ê', 'e');
                texto = texto.Replace('É', 'E');
                texto = texto.Replace('Ê', 'E');
                texto = texto.Replace('õ', 'o');
                texto = texto.Replace('Õ', 'O');
                texto = texto.Replace('ó', 'o');
                texto = texto.Replace('Ó', 'O');
                texto = texto.Replace('ô', 'o');
                texto = texto.Replace('Ô', 'O');
                texto = texto.Replace('ú', 'u');
                texto = texto.Replace('Ú', 'U');
                texto = texto.Replace('ü', 'u');
                texto = texto.Replace('Ü', 'U');
                texto = texto.Replace('í', 'i');
                texto = texto.Replace('Í', 'I');
            }
            #endregion Acentos

            #region Pontuação
            if (pontuacao)
            {
                texto = texto.Replace("'", "");
                texto = texto.Replace("!", "");
                texto = texto.Replace("@", "");
                texto = texto.Replace("#", "");
                texto = texto.Replace("$", "");
                texto = texto.Replace("%", "");
                texto = texto.Replace("¨", "");
                texto = texto.Replace("&", "");
                texto = texto.Replace("*", "");
                texto = texto.Replace("(", "");
                texto = texto.Replace(")", "");
                texto = texto.Replace("-", "");
                texto = texto.Replace("_", "");
                texto = texto.Replace("=", "");
                texto = texto.Replace("+", "");
                texto = texto.Replace("¹", "");
                texto = texto.Replace("²", "");
                texto = texto.Replace("³", "");
                texto = texto.Replace("£", "");
                texto = texto.Replace("¢", "");
                texto = texto.Replace("¬", "");
                texto = texto.Replace("§", "");
                texto = texto.Replace("\"", ""); // "
                texto = texto.Replace("´", "");
                texto = texto.Replace("`", "");
                texto = texto.Replace("{", "");
                texto = texto.Replace("}", "");
                texto = texto.Replace("[", "");
                texto = texto.Replace("]", "");
                texto = texto.Replace("ª", "");
                texto = texto.Replace("º", "");
                texto = texto.Replace("^", "");
                texto = texto.Replace("~", "");
                texto = texto.Replace(",", "");
                texto = texto.Replace(".", "");
                texto = texto.Replace("<", "");
                texto = texto.Replace(">", "");
                texto = texto.Replace(";", "");
                texto = texto.Replace(":", "");
                texto = texto.Replace("/", "");
                texto = texto.Replace("?", "");
                texto = texto.Replace("|", "");
                texto = texto.Replace(@"\", ""); // \
            }
            #endregion Pontuação

            #region Quebra De Linha                
            if (quebralinha)
            {
                texto = texto.Replace(System.Environment.NewLine, "");
                texto = texto.Replace("\r\n", "");
                texto = texto.Replace("\n", "");
            }
            #endregion Quebra De Linha

            #region Espaçamento
            if (espacamento)
                texto = texto.Replace(" ", "");
            #endregion Espaçamento

            #region Trim
            if (trim)
                texto = texto.Trim();
            #endregion Trim

            return texto;
        }
    }
}

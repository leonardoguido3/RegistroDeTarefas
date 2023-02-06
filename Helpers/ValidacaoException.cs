namespace RegistroDeTarefasWebApp.Helpers
{
    public class ValidacaoException : Exception
    {
        public ValidacaoException() { }

        public ValidacaoException(string message)
            : base(message) { }

        public ValidacaoException(string message, Exception inner)
            : base(message, inner) { }
    }
}

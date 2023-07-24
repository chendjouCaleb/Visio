namespace Visio.Parsers.Tex
{
	public class TexError
	{
		public TexErrorType Type { get; set; }
		public TexToken Token { get; set; }
		public string Message { get; set; }

		public TexError(TexErrorType type, TexToken token, string message)
		{
			Type = type;
			Token = token;
			Message = message;
		}

		public TexError(TexErrorType type, TexToken token)
		{
			Type = type;
			Token = token;
		}
	}
}
namespace Crosscutting.DTO.CustomException
{
    public class ExceptionResponseDTO
    {
        public DateTime Date { get; set; }

        public ICollection<ExceptionErrorResponseDTO> Errors { get; set; }

        public class ExceptionErrorResponseDTO
        {
            public string Message { get; set; }

            public ExceptionErrorResponseDTO(string message) => Message = message;
        }

        public ExceptionResponseDTO(string message)
        {
            Date = DateTime.Now;
            Errors = new List<ExceptionErrorResponseDTO>
            {
                new(message)
            };
        }

        public ExceptionResponseDTO(IReadOnlyCollection<string> messages)
        {
            Date = DateTime.Now;
            Errors = new List<ExceptionErrorResponseDTO>();

            foreach (string message in messages)
                Errors.Add(new ExceptionErrorResponseDTO(message));
        }
    }
}

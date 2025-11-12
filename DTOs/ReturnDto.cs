using Cozinhe_Comigo_API.Models;

namespace Cozinhe_Comigo_API.DTOs {
    public class ReturnDto<T> {
        public EInternStatusCode InternStatusCode { get; set; }
        public string ReturnMessage { get; set; } = string.Empty;
        public T? ReturnObject { get; set; }

        public ReturnDto(EInternStatusCode statusCode, string returnMessage, T? returnObject = default) {
            InternStatusCode = statusCode;
            ReturnMessage = returnMessage;
            ReturnObject = returnObject;
        }
    }
}

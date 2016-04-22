using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Boggle
{
    [ServiceContract]
    public interface IBoggleService
    {
        [WebInvoke(Method = "POST", UriTemplate = "/users")]
        Token CreateUser(Username nickname);

        [WebInvoke(Method = "POST", UriTemplate = "/games")]
        GID JoinGame(JoinRequest joinRequest);

        [WebInvoke(Method = "PUT", UriTemplate = "/games")]
        void CancelJoin(Token userToken);

        //[WebInvoke(Method = "PUT", UriTemplate = "/games/{gameID}")]
        //PlayWordScore PlayWord(string gameID, WordPlayed wordPlayed);

        //[WebGet(UriTemplate = "/games/{gameID}?Brief={brief}")]
        //GameStatus GetGameStatus(string gameID, string brief);
    }
}

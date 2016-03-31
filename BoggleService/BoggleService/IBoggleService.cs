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
        string CreateUser(string nickname);

        [WebInvoke(Method = "POST", UriTemplate = "/games")]
        string JoinGame(string userToken, int timeLimit);

        [WebInvoke(Method = "PUT", UriTemplate = "/games")]
        void CancelJoin(string userToken);

        [WebInvoke(Method = "PUT", UriTemplate = "/games/{gameID}")]
        void PlayWord(int gameID);

        [WebGet(UriTemplate = "/games/{gameID}")]
        GameStatus GetGameStatus(int gameID);
    }
}

using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Boggle
{
    [ServiceContract]
    public interface IBoggleService
    {
        /*
        /// <summary>
        /// Sends back index.html as the response body.
        /// </summary>
        [WebGet(UriTemplate = "/api")]
        Stream API();

        /// <summary>
        /// Demo.  You can delete this.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/numbers?length={n}")]
        IList<int> Numbers(string n);

        /// <summary>
        /// Demo.  You can delete this.
        /// </summary>
        [WebInvoke(Method = "POST", UriTemplate = "/first")]
        int GetFirst(IList<int> list);
        */
        [WebInvoke(Method = "POST", UriTemplate = "/users")]
        void CreateUser(string nickname);

        [WebInvoke(Method = "POST", UriTemplate = "/games")]
        void JoinGame(string userToken, int timeLimit);

        [WebInvoke(Method = "PUT", UriTemplate = "/games")]
        void CancelJoin(string userToken);

        [WebInvoke(Method = "PUT", UriTemplate = "/games/{gameID}")]
        void PlayWord(int gameID);

        [WebGet(UriTemplate = "/games/{gameID}")]
        GameStatus GetGameStatus(int gameID);
    }
}

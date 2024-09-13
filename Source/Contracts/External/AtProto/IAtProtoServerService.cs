namespace Contracts.External.AtProto
{
    public interface IAtProtoServerService
    {
        public Task<string> GetAccessToken();
    }
}
namespace Contracts.External.AtProto
{
    public interface IAtProtoRepoService
    {
        public Task CreateRecord(string message);
    }
}
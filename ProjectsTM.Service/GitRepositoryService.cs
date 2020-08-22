using System.Threading.Tasks;

namespace ProjectsTM.Service
{
    public static class GitRepositoryService
    {
        public static Task<bool> CheckRemoteBranchAppDataFile(string filePath)
        {
            Task<bool> task = Task.Run(() =>
            {
                GitCmd.Fetch(filePath);
                return GitCmd.GetDifferentCommitsCount(filePath) > 0;
            }
            );
            return task;            
        }
    }
}

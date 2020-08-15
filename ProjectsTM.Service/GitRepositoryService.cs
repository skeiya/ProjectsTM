using System.Threading.Tasks;

namespace ProjectsTM.Service
{
    public static class GitRepositoryService
    {
        public static Task<bool> CheckRemoteBranchAppDataFile(string filePath)
        {
            return new Task<bool>(() =>
            {
                GitCmd.GitFetch(filePath);
                var localBranchDate = GitCmd.GetLocalBranchDate(filePath);
                if (localBranchDate <= 0) return false;
                var remoteBranchDate = GitCmd.GetRemoteBranchDate(filePath);
                if (remoteBranchDate <= 0) return false;
                if (localBranchDate == remoteBranchDate) return IsRemoteBranchTimeStampNew(filePath);
                return remoteBranchDate > localBranchDate;
            }
            );
        }

        private static bool IsRemoteBranchTimeStampNew(string filePath)
        {
            var localCommitTime = GitCmd.GetLocalBranchCommitTime(filePath);
            if (localCommitTime <= 0) return false;
            var remoteCommitTime = GitCmd.GetRemoteBranchCommitTime(filePath);
            if (remoteCommitTime <= 0) return false;
            return remoteCommitTime > localCommitTime;
        }
    }
}

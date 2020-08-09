using System;
using System.Threading.Tasks;

namespace ProjectsTM.Service
{
    public class GitRepositoryService
    {
        private bool _isRemoteBranchAppDataNew = false;
        private Action<bool> _UpdateMainFormTitlebarText;

        public GitRepositoryService(Action<bool> UpdateMainFormText)
        {
            _UpdateMainFormTitlebarText = UpdateMainFormText;
        }

        public bool IsRemoteBranchAppDataNew
        {
            get { return _isRemoteBranchAppDataNew; }
            set
            {
                _isRemoteBranchAppDataNew = value;
                _UpdateMainFormTitlebarText?.Invoke(_isRemoteBranchAppDataNew);
            }
        }

        internal void CheckRemoteBranchAppDataFile(string filePath)
        {
            Task<bool> task = Task.Run(() =>
            {
                GitCmd.GitFetch(filePath);
                if (DoesRemoteBranchHaveNewCommit(filePath))
                {
                    IsRemoteBranchAppDataNew = true;
                    return true;
                }
                IsRemoteBranchAppDataNew = false;
                return false;
            });
        }    

        private bool DoesRemoteBranchHaveNewCommit(string filePath)
        {
            var localBranchDate = GitCmd.GetLocalBranchDate(filePath);
            if (localBranchDate <= 0) return false;
            var remoteBranchDate = GitCmd.GetRemoteBranchDate(filePath);
            if (remoteBranchDate <= 0) return false;
            if (localBranchDate == remoteBranchDate) return IsRemoteBranchTimeStampNew(filePath);
            return remoteBranchDate > localBranchDate;
        }

        private bool IsRemoteBranchTimeStampNew(string filePath)
        {
            var localCommitTime = GitCmd.GetLocalBranchCommitTime(filePath);
            if (localCommitTime <= 0) return false;
                var remoteCommitTime = GitCmd.GetRemoteBranchCommitTime(filePath);
            if (remoteCommitTime <= 0) return false;
            return remoteCommitTime > localCommitTime;
        }
    }
}

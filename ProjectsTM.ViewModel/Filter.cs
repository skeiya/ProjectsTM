﻿using ProjectsTM.Model;
using System;
using System.Collections.Generic;

namespace ProjectsTM.ViewModel
{
    public class Filter : IEquatable<Filter>
    {
        public Filter() { }
        public Filter(string v, Period period, Members hideMembers, bool isFreeTimeMemberShow, string msFilterSearchPattern)　//TODO:matsukage showMembersを渡すようにする
        {
            if (v != null) WorkItem = v;
            if (period != null) Period = period;
            if (hideMembers != null) HideMembers = hideMembers;　//TODO:matsukage showMembers
            IsFreeTimeMemberShow = isFreeTimeMemberShow;
            if (MSFilterSearchPattern != null) MSFilterSearchPattern = msFilterSearchPattern;
        }

        public Members HideMembers { get; private set; } = new Members(); //TODO:matsukage 過去バージョンを考慮してgetできるようにするが、ShowMembersへ変換する
        public Members ShowMembers { get; private set; } = new Members();
        public Period Period { get; set; } = new Period();
        public string WorkItem { get; set; } = string.Empty;
        public bool IsFreeTimeMemberShow { get; set; } = true;
        public string MSFilterSearchPattern { get; set; } = "ALL";
        public static Filter All => new Filter(null, null, new Members(), false, "ALL");

        public bool Equals(Filter other)
        {
            return other != null &&
                   EqualityComparer<Members>.Default.Equals(HideMembers, other.HideMembers) &&　//TODO:matsukage showMembers
                   EqualityComparer<Period>.Default.Equals(Period, other.Period) &&
                   WorkItem == other.WorkItem &&
                   IsFreeTimeMemberShow == other.IsFreeTimeMemberShow &&
                   MSFilterSearchPattern.Equals(other.MSFilterSearchPattern);
        }

        public override int GetHashCode()
        {
            int hashCode = 69401243;
            hashCode = hashCode * -1521134295 + EqualityComparer<Members>.Default.GetHashCode(HideMembers); //TODO:matsukage ShowMembers
            hashCode = hashCode * -1521134295 + EqualityComparer<Period>.Default.GetHashCode(Period);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(WorkItem);
            hashCode = hashCode * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(IsFreeTimeMemberShow);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MSFilterSearchPattern);
            return hashCode;
        }

        public Filter Clone()
        {
            var result = Filter.All;
            result.HideMembers = this.HideMembers.Clone(); //TODO:ShowMembers
            result.WorkItem = (string)this.WorkItem.Clone();
            result.Period = this.Period.Clone();
            result.IsFreeTimeMemberShow = this.IsFreeTimeMemberShow;
            result.MSFilterSearchPattern = (string)this.MSFilterSearchPattern.Clone();
            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Filter);
        }
    }
}
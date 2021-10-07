using System;
using System.Collections.Generic;

//using Atlassian.Jira;


namespace Global.Models.Jira
{

    public class Issue
    {
        public string Id { get; set; }
        public string Self { get; set; }
        public string Key { get; set; }
        public string Summary { get; set; }
        public Status Status { get; set; }
        public Sprint Sprint { get; set; }
        public Priority Priority { get; set; }
        public Issuetype Issuetype { get; set; }
        public Project Project { get; set; }
        public List<FixVersion> FixVersions { get; set; }
        public object Resolution { get; set; }
        public double? Estimation { get; set; }
        public DateTime? Resolutiondate { get; set; }
        public DateTime Created { get; set; }
        public Epic Epic { get; set; }
        public object Labels { get; set; }
        public Assignee Assignee { get; set; }
        public DateTime Updated { get; set; }
        public string Description { get; set; }
        public bool Flagged { get; set; }
        public Creator Creator { get; set; }
        public Reporter Reporter { get; set; }
        public object ClosedSprints { get; set; }
        public Progress Progress { get; set; }
        public Comment Comment { get; set; }
        public object Parent { get; set; }
        public List<Version> Versions { get; set; }
        public object Components { get; set; }

        //public Attachment Attachment { get; set; }
        //public object Timespent { get; set; }
        //public object Aggregatetimespent { get; set; }
        //public int Workratio { get; set; }
        //public object LastViewed { get; set; }
        //public object Watches { get; set; }
        //public object Timeestimate { get; set; }
        //public object Aggregatetimeoriginalestimate { get; set; }
        //public object Issuelinks { get; set; }
        //public object Timeoriginalestimate { get; set; }
        //public object Timetracking { get; set; }
        //public object Aggregatetimeestimate { get; set; }
        //public object Subtasks { get; set; }
        //public object Aggregateprogress { get; set; }
        //public object Environment { get; set; }
        //public object Votes { get; set; }
        //public object Worklog { get; set; }
        //public object Duedate { get; set; }

    }


    public class IssueDbModel
    {
        public string Expand { get; set; }
        public string Id { get; set; }
        public string Self { get; set; }
        public string Key { get; set; }
        public Fields Fields { get; set; }
    }

    
    public class StatusCategory
    {
        public string Self { get; set; }
        public int Id { get; set; }
        public string Key { get; set; }
        public string ColorName { get; set; }
        public string Name { get; set; }
    }

    public class Status
    {
        public string Self { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public StatusCategory StatusCategory { get; set; }
    }

    public class Priority
    {
        public string Self { get; set; }
        public string IconUrl { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
    }

    public class Issuetype
    {
        public string Self { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public string Name { get; set; }
        public bool Subtask { get; set; }
        public int AvatarId { get; set; }
    }

    public class AvatarUrls
    {
        public string _48x48 { get; set; }
        public string _24x24 { get; set; }
        public string _16x16 { get; set; }
        public string _32x32 { get; set; }
    }

    public class Project
    {
        public string Self { get; set; }
        public string Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public AvatarUrls AvatarUrls { get; set; }
    }

    public class FixVersion
    {
        public string Self { get; set; }
        public string Id { get; set; }
        public object Description { get; set; }
        public string Name { get; set; }
        public bool Archived { get; set; }
        public bool Released { get; set; }
        public object ReleaseDate { get; set; }
    }

    public class Watches
    {
        public string Self { get; set; }
        public int WatchCount { get; set; }
        public bool IsWatching { get; set; }
    }

    public class Color
    {
        public string Key { get; set; }
    }

    public class Epic
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Self { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public Color Color { get; set; }
        public bool Done { get; set; }
    }

    public class Customfield11900
    {
        public string Self { get; set; }
        public string Value { get; set; }
        public string Id { get; set; }
    }

    public class Customfield11704
    {
        public string Self { get; set; }
        public string Value { get; set; }
        public string Id { get; set; }
    }

    public class Assignee
    {
        public string Self { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string EmailAddress { get; set; }
        public AvatarUrls AvatarUrls { get; set; }
        public string DisplayName { get; set; }
        public bool Active { get; set; }
        public string TimeZone { get; set; }
    }

    public class Timetracking
    {
    }

    public class Creator
    {
        public string Self { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string EmailAddress { get; set; }
        public AvatarUrls AvatarUrls { get; set; }
        public string DisplayName { get; set; }
        public bool Active { get; set; }
        public string TimeZone { get; set; }
    }

    public class Fields
    {
        public string Summary { get; set; }
        public Sprint Sprint { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public Issuetype Issuetype { get; set; }
        public object Timespent { get; set; }
        public Project Project { get; set; }
        public List<FixVersion> FixVersions { get; set; }
        //public object Customfield11200 { get; set; }
        //public object Aggregatetimespent { get; set; }
        //public object Customfield11201 { get; set; }
        public Status Resolution { get; set; }
        //public object Customfield11401 { get; set; }
        //public object Customfield11203 { get; set; }
        //public object Customfield11400 { get; set; }
        //public object Customfield10104 { get; set; }
        //public object Customfield10105 { get; set; }
        public double? Customfield_10106 { get; set; }
        //public object Customfield10900 { get; set; }
        public DateTime? Resolutiondate { get; set; }
        public int Workratio { get; set; }
        public object LastViewed { get; set; }
        public Watches Watches { get; set; }
        public DateTime Created { get; set; }
        //public object Customfield12002 { get; set; }
        //public object Customfield12200 { get; set; }
        public Epic Epic { get; set; }
        //public object Customfield12001 { get; set; }
        //public object Customfield10100 { get; set; }
        //public object Customfield12202 { get; set; }
        //public object Customfield12201 { get; set; }
        //public object Customfield12204 { get; set; }
        public object Labels { get; set; }
        //public object Customfield12203 { get; set; }
        //public object Customfield11303 { get; set; }
        //public object Customfield11105 { get; set; }
        //public object Customfield11304 { get; set; }
        //public object Customfield11106 { get; set; }
        //public object Customfield11305 { get; set; }
        //public object Customfield11702 { get; set; }
        //public object Customfield11900 { get; set; }
        //public object Customfield11701 { get; set; }
        //public object Customfield11704 { get; set; }
        public object Timeestimate { get; set; }
        //public object Aggregatetimeoriginalestimate { get; set; }
        //public object Customfield11703 { get; set; }
        //public object Customfield11706 { get; set; }
        //public object Customfield11705 { get; set; }
        //public object Customfield11707 { get; set; }
        public object Issuelinks { get; set; }
        //public object Customfield11709 { get; set; }
        public Assignee Assignee { get; set; }
        public DateTime Updated { get; set; }
        public object Timeoriginalestimate { get; set; }
        public string Description { get; set; }
        //public object Customfield11103 { get; set; }
        //public object Customfield11301 { get; set; }
        //public object Customfield11500 { get; set; }
        //public object Customfield11302 { get; set; }
        //public object Timetracking { get; set; }
        //public object Customfield10401 { get; set; }
        public object Attachment { get; set; }
        public object Aggregatetimeestimate { get; set; }
        public bool Flagged { get; set; }
        public Creator Creator { get; set; }
        public object Subtasks { get; set; }
        public Reporter Reporter { get; set; }
        //public object Customfield11210 { get; set; }
        //public object Customfield11330 { get; set; }
        //public object Customfield10000 { get; set; }
        public object Aggregateprogress { get; set; }
        public object Customfield11211 { get; set; }
        //public object Customfield11212 { get; set; }
        //public object Customfield11006 { get; set; }
        //public object Customfield11403 { get; set; }
        //public object Customfield11601 { get; set; }
        //public object Customfield11204 { get; set; }
        //public object Customfield11402 { get; set; }
        //public object Customfield11205 { get; set; }
        //public object Customfield11600 { get; set; }
        //public object Customfield11405 { get; set; }
        //public object Customfield11206 { get; set; }
        public object Environment { get; set; }
        //public object Customfield11801 { get; set; }
        //public object Customfield11404 { get; set; }
        //public object Customfield11207 { get; set; }
        //public object Customfield11800 { get; set; }
        //public object Customfield11407 { get; set; }
        //public object Customfield11208 { get; set; }
        //public object Customfield11406 { get; set; }
        //public object Customfield11209 { get; set; }
        //public object Customfield11408 { get; set; }
        public object ClosedSprints { get; set; }
        public Progress Progress { get; set; }
        public Comment Comment { get; set; }
        public Votes Votes { get; set; }
        public object Parent { get; set; }
        //public object Customfield10500 { get; set; }
        //public object Customfield10902 { get; set; }
        public List<Version> Versions { get; set; }
        //public object Components { get; set; }
        public object Worklog { get; set; }
        //public object Customfield10700 { get; set; }
        //public object Customfield10701 { get; set; }
        //public object Customfield10410 { get; set; }
        //public object Customfield10411 { get; set; }
        //public object Customfield10402 { get; set; }
        //public object Customfield10403 { get; set; }
        //public object Customfield10800 { get; set; }
        //public object Customfield10404 { get; set; }
        //public object Customfield10405 { get; set; }
        //public object Customfield10406 { get; set; }
        //public object Customfield10407 { get; set; }
        //public object Customfield10408 { get; set; }
        //public object Customfield10409 { get; set; }
        //public object Customfield10400 { get; set; }
        public object Duedate { get; set; }
    }

    public class Subtask
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Self { get; set; }
        public Fields Fields { get; set; }
    }

    public class Reporter
    {
        public string Self { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string EmailAddress { get; set; }
        public AvatarUrls AvatarUrls { get; set; }
        public string DisplayName { get; set; }
        public bool Active { get; set; }
        public string TimeZone { get; set; }
    }

    public class Customfield11330
    {
        public string IssueKey { get; set; }
        public string Status { get; set; }
        public string StatusStyle { get; set; }
        public int Ok { get; set; }
        public double OkPercent { get; set; }
        public string OkJql { get; set; }
        public int Nok { get; set; }
        public double NokPercent { get; set; }
        public string NokJql { get; set; }
        public int Notrun { get; set; }
        public double NotrunPercent { get; set; }
        public string NotRunJql { get; set; }
        public int Unknown { get; set; }
        public double UnknownPercent { get; set; }
        public string UnknownJql { get; set; }
    }

    public class Aggregateprogress
    {
        public int Progress { get; set; }
        public int Total { get; set; }
    }

    public class Customfield11402
    {
        public string Self { get; set; }
        public string Value { get; set; }
        public string Id { get; set; }
    }

    public class Progress
    {
        public int ProgressValue { get; set; }
        public int Total { get; set; }
    }

    public class Comment
    {
        public List<object> Comments { get; set; }
        public int MaxResults { get; set; }
        public int Total { get; set; }
        public int StartAt { get; set; }
    }

    public class Votes
    {
        public string Self { get; set; }
        public int VotesValue { get; set; }
        public bool HasVoted { get; set; }
    }

}

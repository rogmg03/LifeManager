namespace LifeManager.Domain.Enums;

public enum ActivityType
{
    TaskCompleted = 0,
    TaskCreated = 1,
    PhaseStarted = 2,
    PhaseCompleted = 3,
    DocumentCreated = 4,
    DocumentUpdated = 5,
    AttachmentUploaded = 6,
    ReminderFired = 7,
    TimeLogged = 8,
    ProjectCreated = 9,
    ManualEntry = 10
}

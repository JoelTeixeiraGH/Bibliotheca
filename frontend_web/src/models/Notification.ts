export interface Notification {
    notificationId: string;
    notificationTitle: string;
    notificationDescription: string;
    emittedDate: Date;
    endDate: Date;
    forAll: boolean;
    libraryId: number;
}

export interface NotificationDTO {
    notificationTitle: string;
    notificationDescription: string;
    endDate: Date;
    libraryId: number;
}
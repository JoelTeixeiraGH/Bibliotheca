export interface Punishment {
    punishmentId: number,
    punishmentReason: string,
    punishmentLevel: number,
    request: {
        requestId: number,
        requestStatus: number,
        userId: number,
        userName: string
    }
}
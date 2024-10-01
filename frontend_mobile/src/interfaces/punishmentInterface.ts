interface PunishmentType {
  punishmentId: number;
  punishmentReason: number;
  punishmentLevel: string;
  request: {
    requestId: number;
    requestStatus: number;
    userId: number;
    userName: string;
  };
}

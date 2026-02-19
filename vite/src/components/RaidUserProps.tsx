export default interface RaidUserProps{
    userId: number
    userLogin: string
    userName: string
    userPfp: string
    broadcasterType: string
    isBlacklisted: boolean
    raidCount: number
    lastRaidDate: string
    raidedByCount: number
    lastRaidedByDate: string
    isOnline: boolean
    viewCount: number
    gameId: number
    gameName: string
    streamTitle: string
    streamPreview: string
    tags: string[]
    isMature: boolean
    streamStart: string
    slowMode: boolean
    followerMode: boolean
    subscriberMode: boolean
    emoteMode: boolean
    uniqueChatMode: boolean
}
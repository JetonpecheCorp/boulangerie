export type Pagination<T> =
{
    liste: T[],
    total: number,
    numPage: number,
    nbParPage: number,
    aUneProchainePage: boolean
}
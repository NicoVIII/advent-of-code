namespace Day06

type Base = uint64
type Record = { time: Base; distance: Base }

[<RequireQualifiedAccess>]
module Base =
    let inline convertTo x = uint64 x

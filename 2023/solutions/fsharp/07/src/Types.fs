namespace Day07

type CardValueV1 =
    | Two
    | Three
    | Four
    | Five
    | Six
    | Seven
    | Eight
    | Nine
    | Ten
    | Jack
    | Queen
    | King
    | Ace

module CardValueV1 =
    let fromValue =
        function
        | 'A' -> Ace
        | 'K' -> King
        | 'Q' -> Queen
        | 'J' -> Jack
        | 'T' -> Ten
        | '9' -> Nine
        | '8' -> Eight
        | '7' -> Seven
        | '6' -> Six
        | '5' -> Five
        | '4' -> Four
        | '3' -> Three
        | '2' -> Two
        | _ -> failwith "Invalid input!"

type CardValueV2 =
    | Joker
    | Two
    | Three
    | Four
    | Five
    | Six
    | Seven
    | Eight
    | Nine
    | Ten
    | Queen
    | King
    | Ace

[<RequireQualifiedAccess>]
module CardValueV2 =
    let fromValue =
        function
        | 'A' -> Ace
        | 'K' -> King
        | 'Q' -> Queen
        | 'T' -> Ten
        | '9' -> Nine
        | '8' -> Eight
        | '7' -> Seven
        | '6' -> Six
        | '5' -> Five
        | '4' -> Four
        | '3' -> Three
        | '2' -> Two
        | 'J' -> Joker
        | _ -> failwith "Invalid input!"

type Hand<'Value> = 'Value * 'Value * 'Value * 'Value * 'Value

[<RequireQualifiedAccess>]
module Hand =
    let toList<'Value> (hand: Hand<'Value>) =
        let c1, c2, c3, c4, c5 = hand
        [ c1; c2; c3; c4; c5 ]

type HandV1 = Hand<CardValueV1>
type HandV2 = Hand<CardValueV2>

type HandType =
    | HighCard
    | OnePair
    | TwoPair
    | ThreeOfAKind
    | FullHouse
    | FourOfAKind
    | FiveOfAKind

[<RequireQualifiedAccess>]
module HandV1 =
    let toList: (HandV1 -> _) = Hand.toList

    let getType =
        toList
        >> List.groupBy id
        >> List.map (snd >> List.length)
        >> List.sortDescending
        >> function
            | [ 5 ] -> FiveOfAKind
            | [ 4; 1 ] -> FourOfAKind
            | [ 3; 2 ] -> FullHouse
            | [ 3; 1; 1 ] -> ThreeOfAKind
            | [ 2; 2; 1 ] -> TwoPair
            | [ 2; 1; 1; 1 ] -> OnePair
            | [ 1; 1; 1; 1; 1 ] -> HighCard
            | _ -> failwith "Programming error in hand type detection"

    let getHandValue hand =
        let handType = getType hand
        handType, hand

[<RequireQualifiedAccess>]
module HandV2 =
    let toList: (HandV2 -> _) = Hand.toList

    let getType hand =
        let handList = toList hand
        let jokerList, rest = List.partition ((=) Joker) handList
        let joker = List.length jokerList

        let valueList =
            rest |> List.groupBy id |> List.map (snd >> List.length) |> List.sortDescending

        match valueList, joker with
        | [], 5
        | [ _ ], _ -> FiveOfAKind
        | [ 1 ], 4
        | [ _; 1 ], _ -> FourOfAKind
        | [ _; _ ], _ -> FullHouse
        | [ 1; 1 ], 3
        | [ _; 1; 1 ], _ -> ThreeOfAKind
        | [ 2; _; 1 ], _
        | [ 1; 1; 1 ], 2 -> TwoPair
        | [ 1; 1; 1; 1 ], 1
        | [ 2; 1; 1; 1 ], 0 -> OnePair
        | [ 1; 1; 1; 1; 1 ], 0 -> HighCard
        | _ -> failwith "Programming error in hand type detection"

    let getHandValue hand =
        let handType = getType hand
        handType, hand

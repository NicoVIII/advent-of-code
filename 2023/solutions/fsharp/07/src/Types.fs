namespace Day07

type CardValue =
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

type Hand = CardValue * CardValue * CardValue * CardValue * CardValue

type HandType =
    | HighCard
    | OnePair
    | TwoPair
    | ThreeOfAKind
    | FullHouse
    | FourOfAKind
    | FiveOfAKind

module CardValue =
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

[<RequireQualifiedAccess>]
module Hand =
    let toList (hand: Hand) =
        let c1, c2, c3, c4, c5 = hand
        [ c1; c2; c3; c4; c5 ]

    let getType =
        toList
        >> List.groupBy id
        >> List.map (snd >> List.length)
        >> function
            | [ 5 ] -> FiveOfAKind
            | list when List.contains 4 list -> FourOfAKind
            | list when List.contains 3 list && List.contains 2 list -> FullHouse
            | list when List.contains 3 list -> ThreeOfAKind
            | list when List.filter ((=) 2) list |> List.length |> (=) 2 -> TwoPair
            | list when List.contains 2 list -> OnePair
            | list when List.length list = 5 -> HighCard
            | _ -> failwith "Programming error in hand type detection"

    let getHandValue hand =
        let handType = getType hand
        handType, hand

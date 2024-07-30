module Day06.Test

open Expecto
open FsCheck

open Tests

type RecordGen() =
    static member Record() : Arbitrary<Record> =
        Arb.from<Base * Base>
        |> Arb.convert (fun (time, distance) -> { time = time; distance = distance }) (fun record ->
            record.time, record.distance)
        |> Arb.filter (fun record ->
            record.time > 0UL && pown (float record.time / 2.) 2 > float record.distance)

[<AutoOpen>]
module Auto =
    let config = {
        FsCheckConfig.defaultConfig with
            arbitrary = [ typeof<RecordGen> ]
    }

let testProperty = testPropertyWithConfig config

let alternativeImplementation record : Base =
    [ 0UL .. record.time ]
    |> List.filter (fun loadingTime -> (record.time - loadingTime) * loadingTime > record.distance)
    |> List.length
    |> Base.convertTo

[<Tests>]
let unitTests =
    testList (nameof Functions) [
        testList (nameof parseInputV1) [
            let paramList = [
                [ "Time:      7  15   30"; "Distance:  9  40  200" ],
                [
                    { time = 7UL; distance = 9UL }
                    { time = 15UL; distance = 40UL }
                    { time = 30UL; distance = 200UL }
                ]
                [
                    "Time:        40     70     98     79"
                    "Distance:   215   1051   2147   1005"
                ],
                [
                    { time = 40UL; distance = 215UL }
                    { time = 70UL; distance = 1051UL }
                    { time = 98UL; distance = 2147UL }
                    { time = 79UL; distance = 1005UL }
                ]
            ]

            for (input, expected) in paramList do
                test $"{expected}" {
                    Expect.equal (String.concat "\n" input |> parseInputV1) expected ""
                }
        ]
        testList (nameof beatRecordCount) [
            let paramList = [
                { time = 7UL; distance = 9UL }, 4UL
                { time = 30UL; distance = 200UL }, 9UL
                { time = 40UL; distance = 215UL }, 27UL
                { time = 79UL; distance = 1005UL }, 48UL
            ]

            for (record, expected) in paramList do
                test $"{record}" { Expect.equal (beatRecordCount record) expected "" }

                test $"{record} - alternative" {
                    Expect.equal (alternativeImplementation record) expected ""
                }

            testProperty "Alternative implementation" (fun record ->
                beatRecordCount record = alternativeImplementation record)

            testProperty "Always smaller than the whole time" (fun record ->
                beatRecordCount record < record.time)
        ]
    ]

[<Tests>]
let integrationTests =
    {
        day = 6
        part1 = part1 >> string
        part2 = part2 >> string
        readInput = readInput
    }
    |> Tests.build

[<EntryPoint>]
let main argv = runTestsInAssemblyWithCLIArgs [] argv

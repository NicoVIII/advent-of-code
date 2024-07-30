module Day06.Test

open Expecto
open FsCheck

open Tests

type RecordGen() =
    static member Record() : Arbitrary<Record> =
        Arb.from<uint16 * uint16>
        |> Arb.convert (fun (time, distance) -> { time = time; distance = distance }) (fun record ->
            record.time, record.distance)
        |> Arb.filter (fun record ->
            record.time > 0us && pown (float record.time / 2.) 2 > float record.distance)

[<AutoOpen>]
module Auto =
    let config = {
        FsCheckConfig.defaultConfig with
            arbitrary = [ typeof<RecordGen> ]
    }

let testProperty = testPropertyWithConfig config

let alternativeImplementation record =
    [ 0us .. record.time ]
    |> List.filter (fun loadingTime -> (record.time - loadingTime) * loadingTime > record.distance)
    |> List.length
    |> uint16

[<Tests>]
let unitTests =
    testList (nameof Functions) [
        testList (nameof parseInput) [
            let paramList = [
                [ "Time:      7  15   30"; "Distance:  9  40  200" ],
                [
                    { time = 7us; distance = 9us }
                    { time = 15us; distance = 40us }
                    { time = 30us; distance = 200us }
                ]
                [
                    "Time:        40     70     98     79"
                    "Distance:   215   1051   2147   1005"
                ],
                [
                    { time = 40us; distance = 215us }
                    { time = 70us; distance = 1051us }
                    { time = 98us; distance = 2147us }
                    { time = 79us; distance = 1005us }
                ]
            ]

            for (input, expected) in paramList do
                test $"{expected}" {
                    Expect.equal (String.concat "\n" input |> parseInput) expected ""
                }
        ]
        testList (nameof beatRecordCount) [
            let paramList = [
                { time = 7us; distance = 9us }, 4us
                { time = 30us; distance = 200us }, 9us
                { time = 40us; distance = 215us }, 27us
                { time = 79us; distance = 1005us }, 48us
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
        part2 = fun _ -> failwith "Not implemented"
        readInput = readInput
    }
    |> Tests.build

[<EntryPoint>]
let main argv = runTestsInAssemblyWithCLIArgs [] argv

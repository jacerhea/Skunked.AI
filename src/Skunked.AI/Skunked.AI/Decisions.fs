module CardToss

open Combinatorics.Collections
open Skunked.Score
open Skunked.PlayingCards
open System.Collections.Generic

    // Indent all program elements within modules that are declared with an equal sign.
    type AbstractAverageDecision(calculatorIn: ScoreCalculator) = 
        let calculator = calculatorIn

        member this.BaseAverageDecision(hand: seq<Card>) = 
            let handSet = new HashSet<Card>(hand)
            let combinations = new Combinations<Card>(new List<Card>(hand), 4)
            let deck = new Deck()
            let possible = deck |> Seq.except handSet
            this.GetPossibleCombos(combinations, possible)

        member this.GetPossibleCombos(handCombinations: Combinations<Card>, 
                            possibleStarterCards: Collections.seq<Card>) =
                handCombinations 
                |> Seq.map(fun combo -> 
                            let possibleScores = possibleStarterCards |> Seq.map(fun cutCard -> new ScoreWithCut(Cut = cutCard, Score = calculator.CountShowScore(cutCard, combo).Score)) |> Seq.toList
                            new ComboPossibleScores(combo, new List<ScoreWithCut>(possibleScores)))
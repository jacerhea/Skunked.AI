module CardToss

open Combinatorics.Collections
open Skunked.Score
open Skunked.PlayingCards
open System.Collections.Generic

    let calculator = new ScoreCalculator();
    let deck = new Deck()
    let possibleRemaining cards = deck |> Seq.except cards
    let combinations(cards: IEnumerable<Card>) = new Combinations<Card>(new List<Card>(cards), 4)

    let getPossibleCombos(handCombinations: Combinations<Card>, possibleStarterCards: Collections.seq<Card>) =
                handCombinations 
                |> Seq.map(fun combo -> 
                            let possibleScores = possibleStarterCards 
                                                |> Seq.map(fun cutCard -> new ScoreWithCut(Cut = cutCard, Score = calculator.CountShowScore(cutCard, combo).Score)) 
                            new ComboPossibleScores(combo, new List<ScoreWithCut>(possibleScores)))

    // Indent all program elements within modules that are declared with an equal sign.
    let baseDecision(hand: seq<_>) = 
        let handSet = new HashSet<Card>(hand)
        getPossibleCombos(combinations(handSet), possibleRemaining(handSet))

    let maxAverage cards = 
        let highestCombo = baseDecision(cards) 
                        |> Seq.maxBy (fun combo -> combo.GetScoreSummation())
        cards |> Seq.except highestCombo.Combo

    let minAverage cards = 
        let lowestCombo = baseDecision(cards) 
                        |> Seq.minBy (fun combo -> combo.GetScoreSummation())
        cards |> Seq.except lowestCombo.Combo


    let optimisticDecision(cards: IEnumerable<_>) = 
        let handSet = new HashSet<Card>(cards)
        let combinationResult = combinations handSet
        let possible = possibleRemaining handSet
        4
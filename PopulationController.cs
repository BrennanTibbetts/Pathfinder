using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationController : MonoBehaviour
{
    List<GeneticPathfinder> population = new List<GeneticPathfinder>();
    public GameObject creaturePrefab;
    public int populationSize = 100;
    public int genomeLength;
    public float cutoff = 0.3f;
    public Transform spawnPoint;
    public Transform end;
    

    void InitPopulation()
    {
        for(int i = 0; i < populationSize; i++)
        {
            GameObject go = Instantiate(creaturePrefab, spawnPoint.position, Quaternion.identity);
            go.GetComponent<GeneticPathfinder>().InitCreature(new DNA(genomeLength), end.position);
            population.Add(go.GetComponent<GeneticPathfinder>());
        }
    }
    void NextGeneration()
    {
        int survivorCut = Mathf.RoundToInt(populationSize * cutoff);
        List<GeneticPathfinder> survivors = new List<GeneticPathfinder>();
        for(int i = 0; i < survivorCut; i++)
        {
            survivors.Add(GetFittest());
        }    
        for(int i = 0; i < population.Count; i++)
        {
            Destroy(population[i].gameObject);
        }
        population.Clear();

        while(population.Count < populationSize)
        {
            for(int i = 0; i < survivors.Count; i++)
            {
                GameObject go = Instantiate(creaturePrefab, spawnPoint.position, Quaternion.identity);
                go.GetComponent<GeneticPathfinder>().InitCreature(new DNA(survivors[i].dna, survivors[Random.Range(0,10)].dna), end.position);
                population.Add(go.GetComponent<GeneticPathfinder>());
                if(population.Count >= populationSize)
                {
                    break;
                }
            }
        }
        for(int i = 0; i < survivors.Count; i++)
        {
            Destroy(survivors[i].gameObject);
        }
    }
    void Awake()
    {
        InitPopulation();
    }
    void Update()
    {
        if(!HasActive())
        {
            NextGeneration();
        }
    }
    GeneticPathfinder GetFittest()
    {
        float maxFitness = float.MinValue;
        int index = 0;
        for(int p = 0; p < population.Count; p++)
        {
            if(population[p].fitness > maxFitness)
            {
                maxFitness = population[p].fitness;
                index = p;
            }
        }
        GeneticPathfinder fittest = population[index];
        population.Remove(fittest);
        return fittest;
    }
    bool HasActive()
    {
        for(int i = 0; i < population.Count; i++)
        {
            if(!population[i].hasFinished)
            {
                return true;
            }
        }
        return false;        
    }
}

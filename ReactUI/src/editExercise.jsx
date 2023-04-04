import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from 'react-router-dom';

function EditExercise(){
    const [error, setError] = useState(null);
    const [isLoaded, setIsLoaded] = useState(false);
    
    const [exerciseName, setExName] = useState("");
    const [sets, setSets] = useState("");
    const [reps, setReps] = useState("");
    const [weight, setWeight] = useState("");
    const [instructions, setExInstr] = useState("");
    const [idexercise, setExID] = useState("");

    let { exerciseID } = useParams();
    const navigate = useNavigate();

    useEffect(() => {
        fetch(`http://localhost:5000/Exercises/GetExerciseId?id=${exerciseID}`)
        .then(res => res.json())
        .then(
            (result) => {
                setExID(result.idexercise)
                setSets(result.sets);
                setReps(result.reps);
                setWeight(result.weight);
                setExName(result.exerciseName);
                setExInstr(result.instructions);
                setIsLoaded(true);
            },
            (error) => {
                setIsLoaded(true);
                setError(error);
        }
        )
    }, [exerciseID])

    const gotoExercises = () => {
      navigate("/Exercises");
    }

    let handleSubmit = async (e) => {
      e.preventDefault();
      try {
        let res = await fetch("http://localhost:5000/Exercises/SaveExercise", {
          method: "POST",
          headers: {
    'Content-Type': 'application/json'
    },
          body: JSON.stringify({
            idexercise: idexercise,
            exerciseName: exerciseName,
            sets: sets,
            reps: reps,
            weight: weight,
            instructions: instructions,
          }),
        });
        if(res.status === 200) {
          gotoExercises();
        }
      } catch(err) {
        console.log(err);
      }
    };

    if (error) {
    return <div>Error: {error.message}</div>;
  } else if (!isLoaded) {
    return <div>Loading...</div>;
  } else {
    return (
        <>
        <label>Exercise Name</label>
        <input type="text" id="name" defaultValue={exerciseName} onChange={(e) => setExName(e.target.value)} />
        <label>Sets</label>
        <input type="text" id="sets" defaultValue={sets} onChange={(e) => setSets(Number(e.target.value))} />
        <label>Reps</label>
        <input type="text" id="reps" defaultValue={reps} onChange={(e) => setReps(Number(e.target.value))} />
        <label>Weight</label>
        <input type="text" id="weight" defaultValue={weight} onChange={(e) => setWeight(Number(e.target.value))} />
        <label>Exercise Instructions</label>
        <input type="text" id="desc" defaultValue={instructions} onChange={(e) => setExInstr(e.target.value)} />
        <button onClick={handleSubmit}>Save</button>
        <button onClick={gotoExercises}>Cancel Edit</button>
        </>
    );
  }
}

export default EditExercise;
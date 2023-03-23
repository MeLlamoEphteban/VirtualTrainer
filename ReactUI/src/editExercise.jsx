import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from 'react-router-dom';
import { post } from "jquery";

function EditExercise(){
    const [error, setError] = useState(null);
    const [isLoaded, setIsLoaded] = useState(false);
    const [item, setItem] = useState([]);

    const [exerciseName, setExName] = useState("");
    const [sets, setSets] = useState("");
    const [reps, setReps] = useState("");
    const [weight, setWeight] = useState("");
    const [instructions, setExInstr] = useState("");

    let { exerciseID } = useParams();
    const navigate = useNavigate();

    useEffect(() => {
        fetch(`http://localhost:5000/Exercises/GetExerciseId?id=${exerciseID}`)
        .then(res => res.json())
        .then(
            (result) => {
                setIsLoaded(true);
                setItem(result);
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
          body: JSON.stringify({
            exerciseName: exerciseName,
            sets: sets,
            reps: reps,
            weight: weight,
            instructions: instructions,
          }),
        });
        let resJson = await res.json();
        if(res.status === 200) {
          exerciseName("");
          sets("");
          reps("");
          weight("");
          instructions("");
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
        <form onSubmit={handleSubmit}>
        <label>Exercise Name</label>
        <input type="text" id="name" defaultValue={item.exerciseName} onChange={(e) => setExName(e.target.value)} />
        <label>Sets</label>
        <input type="text" id="sets" defaultValue={item.sets} onChange={(e) => setSets(Number(e.target.value))} />
        <label>Reps</label>
        <input type="text" id="reps" defaultValue={item.reps} onChange={(e) => setReps(Number(e.target.value))} />
        <label>Weight</label>
        <input type="text" id="weight" defaultValue={item.weight} onChange={(e) => setWeight(Number(e.target.value))} />
        <label>Exercise Instructions</label>
        <input type="text" id="desc" defaultValue={item.instructions} onChange={(e) => setExInstr(e.target.value)} />
        <button type="submit">Save</button>
        <button onClick={gotoExercises}>Cancel Edit</button>
        </form>
        </>
    );
  }
}

export default EditExercise;
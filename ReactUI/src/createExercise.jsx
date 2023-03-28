import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

function CreateExercise(){
    const [exerciseName, setExName] = useState("");
    const [sets, setSets] = useState("");
    const [reps, setReps] = useState("");
    const [weight, setWeight] = useState("");
    const [instructions, setExInstr] = useState("");
    const navigate = useNavigate();

    const gotoExercises =() => {
        navigate("/Exercises");
    }

    let handleSubmit =async (e) => {
        e.preventDefault();
        try {
            let res = await fetch("http://localhost:5000/Exercises/CreateExercise", {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json'
                },

                body: JSON.stringify({
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

    return (
        <>
        <form onSubmit={handleSubmit}>
        <label>Exercise Name</label>
        <input type="text" id="name" onChange={(e) => setExName(e.target.value)} />
        <label>Sets</label>
        <input type="text" id="sets" onChange={(e) => setSets(Number(e.target.value))} />
        <label>Reps</label>
        <input type="text" id="reps" onChange={(e) => setReps(Number(e.target.value))} />
        <label>Weight</label>
        <input type="text" id="weight" onChange={(e) => setWeight(Number(e.target.value))} />
        <label>Exercise Instructions</label>
        <input type="text" id="desc" onChange={(e) => setExInstr(e.target.value)} />
        <button type="submit">Create</button>
        <button onClick={gotoExercises}>Cancel Create</button>
        </form>
        </>
    )
}

export default CreateExercise;
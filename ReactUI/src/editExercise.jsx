import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from 'react-router-dom';
import { post } from "jquery";

function EditExercise(){
    const [error, setError] = useState(null);
    const [isLoaded, setIsLoaded] = useState(false);
    const [item, setItem] = useState([]);

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
    
    function Form() {
      const [exName, setExName] = useState('');
      const [exInstr, setExInsts] = useState('');
      const [reps, setReps] = useState('');
      const [sets, setSets] = useState('');
      const [weight, setWeight] = useState('');

      useEffect(() => {
        post('http://localhost:5000/Exercises/GetExercisesRaw', { eventName: 'exerciseForm' });
      }, []);

      function handleSubmit(e) {
        e.preventDefault();
        post('http://localhost:5000/Exercises/SaveExercise', { exName, exInstr, reps, sets, weight });
      }
    }

    if (error) {
    return <div>Error: {error.message}</div>;
  } else if (!isLoaded) {
    return <div>Loading...</div>;
  } else {
    return (
        <>
        <form id="exerciseForm">
        <label>Exercise Name</label>
        <input type="text" id="name" defaultValue={item.exerciseName} />
        <label>Exercise Instructions</label>
        <input type="text" id="desc" defaultValue={item.instructions} />
        <label>Reps</label>
        <input type="text" id="reps" defaultValue={item.reps} />
        <label>Sets</label>
        <input type="text" id="sets" defaultValue={item.sets} />
        <label>Weight</label>
        <input type="text" id="weight" defaultValue={item.weight} />
        <button type="submit" onClick={Form}>Save</button>
        <button onClick={gotoExercises}>Cancel Edit</button>
        </form>
        </>
    );
  }
}

export default EditExercise;
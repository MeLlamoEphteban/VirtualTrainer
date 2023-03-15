import { BrowserRouter, Route, Routes, Link } from 'react-router-dom';
import Home from './pages/Home';
import Exercises from './pages/Exercises'
import EditExercise from './editExercise';

export default function Navbar() {
    return (
        <>
        <BrowserRouter>
        <nav className="nav">
            <Link to="/Home" className='site-title'>Virtual Trainer</Link>
            <ul>
                <li className='active'><Link to="/Exercises">Exercises</Link></li>
            </ul>
        </nav>
        <Routes>
            <Route path='/Home' element={<Home />}>Virtual Trainer</Route>
            <Route path='/Exercises' element={<Exercises />}>Exercises</Route>
            <Route path='/Exercises/Edit/:exerciseID' element={<EditExercise />}></Route>
        </Routes>
        </BrowserRouter>
        </>
    )
}
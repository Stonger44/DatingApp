import { Photo } from './photo';

export interface User {
    id: number;
    username: string;
    gender: string;
    age: string;
    knownAs: string;
    created: Date;
    lastActive: Date;
    country: string;
    city: string;
    photoUrl: string;
    introduction?: string;
    lookingFor?: string;
    interests?: string;
    photos?: Photo[];
}

export type TExercise = {
    id: number;
	name: string;
	description: string;
	category: string;
    difficulty: number,
	grade: {
		color: number;
		degrees: number;
	};
	beltLevelMin: {
		color: number;
		degrees: number;
	};
	beltLevelMax: {
		color: number;
		degrees: number;
	};
	duration: string;
	minYearsOfTraining: number;
    videoURL: string;
};
